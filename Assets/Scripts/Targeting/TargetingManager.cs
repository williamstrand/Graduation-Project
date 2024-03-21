using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WSP.Input;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Targeting
{
    public class TargetingManager : MonoBehaviour
    {
        static TargetingManager instance;
        static UnityEngine.Camera mainCamera;

        static Vector2Int currentOrigin;
        static Vector2Int currentTarget;

        static IAction currentAction;
        static IPlayerUnitController currentPlayerController;
        static bool isTargeting;

        public static Color NormalColor => instance.normalColor;
        public static Color FriendlyColor => instance.friendlyColor;
        public static Color EnemyColor => instance.enemyColor;

        [SerializeField] TargetingReticle reticle;
        [SerializeField] LineRenderer lineRenderer;

        [Header("Colors")]
        [SerializeField] Color normalColor = Color.grey;
        [SerializeField] Color friendlyColor = Color.green;
        [SerializeField] Color enemyColor = Color.red;

        void Awake()
        {
            instance = this;
            mainCamera = UnityEngine.Camera.main;
        }

        public static void StartActionTargeting(IPlayerUnitController origin, IAction action)
        {
            currentPlayerController = origin;
            currentAction = action;

            InputHandler.Controls.Game.Target.performed += ExecuteAction;
            InputHandler.Controls.Game.CancelTarget.performed += CancelTargeting;

            isTargeting = true;
        }

        public static void Target(Vector2Int origin, Vector2Int target)
        {
            var type = GetReticleTargetType(origin, target);
            if (currentOrigin == origin && currentTarget == target && instance.reticle.Type == type) return;

            currentOrigin = origin;
            currentTarget = target;
            instance.reticle.Type = type;

            if (isTargeting)
            {
                HidePath();
                currentAction.TargetingType.Target(currentOrigin, currentTarget, instance.reticle);
                return;
            }

            SetReticlePosition();
            DrawPath();
        }

        static void SetReticlePosition()
        {
            if (isTargeting) return;

            if (instance.reticle.Type == TargetingReticle.ReticleTargetType.None)
            {
                instance.reticle.Enable(false);
                HidePath();
                return;
            }

            instance.reticle.Type = instance.reticle.Type;
            instance.reticle.SetPosition(currentTarget);
            instance.reticle.Enable(true);
        }

        static void CancelTargeting(InputAction.CallbackContext _)
        {
            InputHandler.Controls.Game.Target.performed -= ExecuteAction;
            InputHandler.Controls.Game.CancelTarget.performed -= CancelTargeting;

            currentAction.TargetingType.StopTarget();
            isTargeting = false;
            instance.reticle.Enable(false);
        }

        static void ExecuteAction(InputAction.CallbackContext _)
        {
            if (!isTargeting) return;

            var mousePosition = InputHandler.Controls.General.MousePosition.ReadValue<Vector2>();
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);
            var target = new ActionTarget
            {
                TargetPosition = gridPosition
            };

            if (GameManager.CurrentLevel.IsOccupied(gridPosition))
            {
                target.TargetUnit = GameManager.CurrentLevel.GetUnitAt(gridPosition);
            }

            var actionContext = new ActionContext(currentAction, target);

            if (!currentPlayerController.StartAction(actionContext)) return;

            CancelTargeting(_);
        }

        static void DrawPath()
        {
            if (isTargeting)
            {
                HidePath();
                return;
            }

            if (GameManager.CurrentLevel.Map.GetValue(currentOrigin) == Map.Pathfinding.Map.Wall)
            {
                HidePath();
                return;
            }

            if (currentOrigin == currentTarget)
            {
                HidePath();
                return;
            }

            if (GameManager.CurrentLevel.FindPath(currentOrigin, currentTarget, out var path))
            {
                instance.lineRenderer.positionCount = path.Count;
                var offset = new Vector2(GameManager.CurrentLevel.Map.CellSize / 2, GameManager.CurrentLevel.Map.CellSize / 2);

                for (var i = 0; i < path.Count; i++)
                {
                    instance.lineRenderer.SetPosition(i, path[i].Position + offset);
                }

                var color = instance.reticle.Type switch
                {
                    TargetingReticle.ReticleTargetType.Normal => instance.normalColor,
                    TargetingReticle.ReticleTargetType.Friendly => instance.friendlyColor,
                    TargetingReticle.ReticleTargetType.Enemy => instance.enemyColor,
                    TargetingReticle.ReticleTargetType.None => Color.clear,
                    _ => throw new ArgumentOutOfRangeException(nameof(instance.reticle.Type), instance.reticle.Type, null)
                };

                instance.lineRenderer.startColor = color;
                instance.lineRenderer.endColor = color;
            }
            else
            {
                HidePath();
            }
        }

        static void HidePath()
        {
            instance.lineRenderer.positionCount = 0;
        }

        static TargetingReticle.ReticleTargetType GetReticleTargetType(Vector2Int origin, Vector2Int position)
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