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

        static TargetingReticle.ReticleTargetType currentType;
        static UnityEngine.Camera mainCamera;
        static Vector2Int currentOrigin;
        static Vector2Int currentPosition;
        static IAction currentAction;
        static IPlayerUnitController currentPlayerController;
        static ActionTarget currentTarget;
        static TargetingType currentTargetingType;
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

        public static void StartTargeting(IPlayerUnitController origin, TargetingType targetingType, IAction action)
        {
            currentPlayerController = origin;
            currentTargetingType = targetingType;
            currentAction = action;

            InputHandler.Controls.Game.Target.performed += Execute;
            InputHandler.Controls.Game.CancelTarget.performed += CancelTargeting;

            isTargeting = true;
        }

        static void CancelTargeting(InputAction.CallbackContext context)
        {
            InputHandler.Controls.Game.Target.performed -= Execute;
            InputHandler.Controls.Game.CancelTarget.performed -= CancelTargeting;

            isTargeting = false;
            instance.reticle.Enable(false);
        }

        static void SetTargeting(Vector2Int position, TargetingReticle.ReticleTargetType type)
        {
            switch (currentTargetingType)
            {
                case TargetingType.Unit:
                    if (type == TargetingReticle.ReticleTargetType.Enemy)
                    {
                        instance.reticle.SetPosition(position, TargetingReticle.ReticleTargetType.Enemy);
                        break;
                    }

                    instance.reticle.Enable(false);
                    break;

                case TargetingType.Position:
                    instance.reticle.SetPosition(position, type);
                    break;

                case TargetingType.Line:
                    instance.reticle.SetPosition(position, type);
                    break;
            }
        }

        static void Execute(InputAction.CallbackContext context)
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

            InputHandler.Controls.Game.Target.performed -= Execute;
            isTargeting = false;
        }

        public static void SetTargetPosition(Vector2Int origin, Vector2Int position)
        {
            var type = GetReticleTargetType(origin, position);
            if (currentOrigin == origin && currentPosition == position && currentType == type) return;

            currentOrigin = origin;
            currentPosition = position;
            currentType = type;

            // Set targeting
            if (isTargeting)
            {
                instance.lineRenderer.positionCount = 0;
                SetTargeting(position, type);
                return;
            }

            if (type == TargetingReticle.ReticleTargetType.None)
            {
                instance.reticle.Enable(false);
                instance.lineRenderer.positionCount = 0;
                return;
            }

            instance.reticle.SetPosition(position, type);
            instance.reticle.Enable(true);

            // Draw path
            DrawPath(origin, position, type);
        }

        static void DrawPath(Vector2Int origin, Vector2Int position, TargetingReticle.ReticleTargetType type)
        {
            if (GameManager.CurrentLevel.Map.GetValue(origin) == Map.Pathfinding.Map.Wall)
            {
                instance.lineRenderer.positionCount = 0;
                return;
            }

            if (GameManager.CurrentLevel.FindPath(origin, position, out var path))
            {
                instance.lineRenderer.positionCount = path.Count;
                var offset = new Vector2(GameManager.CurrentLevel.Map.CellSize / 2, GameManager.CurrentLevel.Map.CellSize / 2);

                for (var i = 0; i < path.Count; i++)
                {
                    instance.lineRenderer.SetPosition(i, path[i].Position + offset);
                }

                var color = type switch
                {
                    TargetingReticle.ReticleTargetType.Normal => instance.normalColor,
                    TargetingReticle.ReticleTargetType.Friendly => instance.friendlyColor,
                    TargetingReticle.ReticleTargetType.Enemy => instance.enemyColor,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                instance.lineRenderer.startColor = color;
                instance.lineRenderer.endColor = color;
            }
            else
            {
                instance.lineRenderer.positionCount = 0;
            }
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