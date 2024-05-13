using System;
using UnityEngine;
using UnityEngine.EventSystems;
using WSP.Input;
using WSP.Targeting.TargetingTypes;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Targeting
{
    public class TargetingComponent : MonoBehaviour
    {
        public Vector2Int CurrentTarget { get; private set; }
        Vector2Int currentOrigin;

        public IAction CurrentAction { get; private set; }
        PlayerController playerController;
        public bool InTargetSelectionMode { get; private set; }

        bool shouldDrawPath = true;

        [field: SerializeField] public TargetingReticle Reticle { get; private set; }
        LineRenderer lineRenderer;

        TargetingType DefaultTargetingType { get; } = new PositionTargeting();
        TargetingType currentTargetingType;

        [field: Header("Colors")]
        [field: SerializeField] public Color NormalColor { get; private set; } = Color.grey;
        [field: SerializeField] public Color FriendlyColor { get; private set; } = Color.green;
        [field: SerializeField] public Color EnemyColor { get; private set; } = Color.red;

        bool isHoveringUi;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            playerController = GetComponent<PlayerController>();
        }

        void Start()
        {
            SetTargetingType(DefaultTargetingType);
        }

        void Update()
        {
            isHoveringUi = EventSystem.current.IsPointerOverGameObject();

            var mousePosition = InputHandler.MousePosition;
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(mousePosition);
            Target(gridPosition);

            DrawPath();
        }

        public void StartActionTargeting(IAction action)
        {
            CurrentAction = action;

            InputHandler.OnTarget += ExecuteAction;
            InputHandler.OnCancel += CancelTargeting;

            InTargetSelectionMode = true;
            SetTargetingType(CurrentAction.TargetingType);
        }

        void SetTargetingType(TargetingType targetingType)
        {
            currentTargetingType?.StopTarget();
            currentTargetingType = targetingType;
            currentTargetingType.StartTarget(this);

            CurrentTarget = Vector2Int.zero;
            currentOrigin = playerController.Unit.GridPosition;
        }

        void Target(Vector2Int target)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var type = GetReticleTargetType(playerController.Unit.GridPosition, target);
            if (currentOrigin == playerController.Unit.GridPosition && CurrentTarget == target && Reticle.Type == type) return;

            currentOrigin = playerController.Unit.GridPosition;
            CurrentTarget = target;
            Reticle.Type = type;

            currentTargetingType.Target(currentOrigin, CurrentTarget);
        }

        void CancelTargeting()
        {
            InputHandler.OnTarget -= ExecuteAction;
            InputHandler.OnCancel -= CancelTargeting;

            SetTargetingType(DefaultTargetingType);

            InTargetSelectionMode = false;
        }

        void ExecuteAction(Vector2 position)
        {
            if (isHoveringUi) return;
            if (!InTargetSelectionMode) return;

            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(position);
            var actionContext = new ActionContext(CurrentAction, gridPosition);

            if (!playerController.StartAction(actionContext)) return;

            CancelTargeting();
        }

        void DrawPath()
        {
            if (!shouldDrawPath) return;

            if (GameManager.CurrentLevel.Map.GetValue(currentOrigin) == Map.Pathfinding.Map.Wall)
            {
                lineRenderer.positionCount = 0;
                return;
            }

            if (currentOrigin == CurrentTarget)
            {
                lineRenderer.positionCount = 0;
                return;
            }

            if (GameManager.CurrentLevel.FindPath(currentOrigin, CurrentTarget, out var path))
            {
                lineRenderer.positionCount = path.Count;
                var offset = new Vector2(GameManager.CurrentLevel.Map.CellSize / 2, GameManager.CurrentLevel.Map.CellSize / 2);

                for (var i = 0; i < path.Count; i++)
                {
                    lineRenderer.SetPosition(i, path[i].Position + offset);
                }

                var color = Reticle.Type switch
                {
                    TargetingReticle.ReticleTargetType.Normal => NormalColor,
                    TargetingReticle.ReticleTargetType.Friendly => FriendlyColor,
                    TargetingReticle.ReticleTargetType.Enemy => EnemyColor,
                    TargetingReticle.ReticleTargetType.None => Color.clear,
                    _ => throw new ArgumentOutOfRangeException(nameof(Reticle.Type), Reticle.Type, null)
                };

                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }

        public void HidePath()
        {
            shouldDrawPath = false;
            lineRenderer.positionCount = 0;
        }

        public void ShowPath()
        {
            shouldDrawPath = true;
        }

        static TargetingReticle.ReticleTargetType GetReticleTargetType(Vector2Int origin, Vector2Int position)
        {
            if (GameManager.CurrentLevel.FogOfWar.IsHidden(position))
            {
                return TargetingReticle.ReticleTargetType.None;
            }

            if (GameManager.CurrentLevel.IsOccupied(position))
            {
                var isOrigin = origin == position;
                return isOrigin ? TargetingReticle.ReticleTargetType.None : TargetingReticle.ReticleTargetType.Enemy;
            }

            var isWall = GameManager.CurrentLevel.Map.GetValue(position) == Map.Pathfinding.Map.Wall;
            return isWall ? TargetingReticle.ReticleTargetType.None : TargetingReticle.ReticleTargetType.Normal;
        }
    }
}