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

        public static Color NormalColor => instance.normalColor;
        public static Color FriendlyColor => instance.friendlyColor;
        public static Color EnemyColor => instance.enemyColor;

        [SerializeField] TargetingReticle reticle;
        [SerializeField] LineRenderer lineRenderer;

        [Header("Colors")]
        [SerializeField] Color normalColor = Color.grey;
        [SerializeField] Color friendlyColor = Color.green;
        [SerializeField] Color enemyColor = Color.red;

        TargetingReticle.ReticleTargetType currentType;
        static UnityEngine.Camera mainCamera;
        Vector2Int currentOrigin;
        Vector2Int currentPosition;
        IAction currentAction;
        IPlayerUnitController currentPlayerController;
        ActionTarget currentTarget;
        TargetingType currentTargetingType;
        bool isTargeting;

        void Awake()
        {
            instance = this;
            mainCamera = UnityEngine.Camera.main;
        }


        public static void StartTargeting(IPlayerUnitController origin, TargetingType targetingType, IAction action)
        {
            instance.currentPlayerController = origin;
            instance.currentTargetingType = targetingType;
            instance.reticle.Enable(true);
            instance.currentAction = action;
            InputHandler.Controls.Game.Target.performed += Execute;
            instance.isTargeting = true;
        }

        static void Execute(InputAction.CallbackContext context)
        {
            if (!instance.isTargeting) return;

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

            var actionContext = new ActionContext(instance.currentAction, target);

            if (instance.currentPlayerController.StartAction(actionContext))
            {
                Debug.Log("Execute");
                InputHandler.Controls.Game.Target.performed -= Execute;
                instance.isTargeting = false;
            }
        }

        public static void SetTargetPosition(Vector2Int origin, Vector2Int position, TargetingReticle.ReticleTargetType type = TargetingReticle.ReticleTargetType.None)
        {
            if (instance.currentOrigin == origin && instance.currentPosition == position && instance.currentType == type) return;

            instance.currentOrigin = origin;
            instance.currentPosition = position;
            instance.currentType = type;

            if (type == TargetingReticle.ReticleTargetType.None)
            {
                instance.reticle.SetPosition(position, type);
                instance.reticle.Enable(false);
                instance.lineRenderer.positionCount = 0;
                return;
            }

            instance.reticle.Enable(true);

            if (instance.isTargeting)
            {
                switch (instance.currentTargetingType)
                {
                    case TargetingType.Unit:
                        if (type == TargetingReticle.ReticleTargetType.Enemy) instance.reticle.SetPosition(position, TargetingReticle.ReticleTargetType.Enemy);
                        break;

                    case TargetingType.Position:
                        instance.reticle.SetPosition(position, type);
                        break;

                    case TargetingType.Line:
                        instance.reticle.SetPosition(position, type);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return;
            }

            instance.reticle.SetPosition(position, type);

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
    }
}