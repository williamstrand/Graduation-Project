using System;
using UnityEngine;
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

        IAction currentAction;
        IPlayerUnitController playerController;
        public bool InTargetSelectionMode { get; private set; }

        public bool ShouldDrawPath { get; set; } = true;

        [field: SerializeField] public TargetingReticle Reticle { get; private set; }
        LineRenderer lineRenderer;

        TargetingType DefaultTargetingType { get; } = new DefaultTargeting();
        TargetingType currentTargetingType;

        [field: Header("Colors")]
        [field: SerializeField] public Color NormalColor { get; private set; } = Color.grey;
        [field: SerializeField] public Color FriendlyColor { get; private set; } = Color.green;
        [field: SerializeField] public Color EnemyColor { get; private set; } = Color.red;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            playerController = GetComponent<IPlayerUnitController>();
        }

        void Start()
        {
            SetTargetingType(DefaultTargetingType);
        }

        void Update()
        {
            var mousePosition = InputHandler.MousePosition;
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(mousePosition);
            Target(gridPosition);

            if (ShouldDrawPath)
            {
                DrawPath();
            }
            else
            {
                HidePath();
            }
        }

        public void StartActionTargeting(IAction action)
        {
            if (action.TargetingType is SelfTargeting)
            {
                var actionContext = new ActionContext(action, Vector2Int.zero);
                playerController.StartAction(actionContext);
                return;
            }

            currentAction = action;

            InputHandler.OnTarget += ExecuteAction;
            InputHandler.OnCancel += CancelTargeting;

            InTargetSelectionMode = true;
            SetTargetingType(currentAction.TargetingType);
        }

        void SetTargetingType(TargetingType targetingType)
        {
            currentTargetingType?.StopTarget();
            currentTargetingType = targetingType;
            currentTargetingType.StartTarget(this);
        }

        void Target(Vector2Int target)
        {
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
            if (!InTargetSelectionMode) return;

            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(position);
            var actionContext = new ActionContext(currentAction, gridPosition);

            if (!playerController.StartAction(actionContext)) return;

            CancelTargeting();
        }

        void DrawPath()
        {
            if (GameManager.CurrentLevel.Map.GetValue(currentOrigin) == Map.Pathfinding.Map.Wall)
            {
                HidePath();
                return;
            }

            if (currentOrigin == CurrentTarget)
            {
                HidePath();
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
                HidePath();
            }
        }

        void HidePath()
        {
            lineRenderer.positionCount = 0;
        }

        TargetingReticle.ReticleTargetType GetReticleTargetType(Vector2Int origin, Vector2Int position)
        {
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