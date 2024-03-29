﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using WSP.Input;
using WSP.Targeting.TargetingTypes;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Targeting
{
    public class TargetingManager : MonoBehaviour
    {
        static TargetingManager instance;

        UnityEngine.Camera mainCamera;

        Vector2Int currentOrigin;
        Vector2Int currentTarget;

        IAction currentAction;
        IPlayerUnitController currentPlayerController;
        public static bool InTargetSelectionMode { get; private set; }
        bool updateInTargetSelectionMode;

        [HideInInspector] public bool ShouldDrawPath = true;

        [field: SerializeField] public TargetingReticle Reticle { get; private set; }
        [SerializeField] LineRenderer lineRenderer;

        TargetingType defaultTargetingType = new DefaultTargeting();
        TargetingType currentTargetingType;

        [field: Header("Colors")]
        [field: SerializeField] public Color NormalColor { get; private set; } = Color.grey;
        [field: SerializeField] public Color FriendlyColor { get; private set; } = Color.green;
        [field: SerializeField] public Color EnemyColor { get; private set; } = Color.red;

        Vector2Int MousePosition
        {
            get
            {
                var mousePosition = InputHandler.Controls.General.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                return GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);
            }
        }

        void Awake()
        {
            instance = this;
            mainCamera = UnityEngine.Camera.main;
            SetTargetingType(defaultTargetingType);
        }

        void Update()
        {
            Target(GameManager.CurrentLevel.Player.Unit.GridPosition, MousePosition);

            if (ShouldDrawPath)
            {
                DrawPath();
            }
            else
            {
                HidePath();
            }
        }

        void LateUpdate()
        {
            if (!updateInTargetSelectionMode) return;

            InTargetSelectionMode = !InTargetSelectionMode;
            updateInTargetSelectionMode = false;
        }

        public static Vector2Int GetTarget()
        {
            return instance.currentTarget;
        }

        public static void StartActionTargeting(IPlayerUnitController origin, IAction action)
        {
            instance.currentPlayerController = origin;
            instance.currentAction = action;

            InputHandler.Controls.Game.Target.performed += instance.ExecuteAction;
            InputHandler.Controls.Game.CancelTarget.performed += instance.CancelTargeting;

            InTargetSelectionMode = true;
            instance.SetTargetingType(instance.currentAction.TargetingType);
        }

        void SetTargetingType(TargetingType targetingType)
        {
            currentTargetingType?.StopTarget();
            currentTargetingType = targetingType;
            currentTargetingType.StartTarget(this);
        }

        void Target(Vector2Int origin, Vector2Int target)
        {
            var type = GetReticleTargetType(origin, target);
            if (currentOrigin == origin && currentTarget == target && Reticle.Type == type) return;

            currentOrigin = origin;
            currentTarget = target;
            Reticle.Type = type;

            currentTargetingType.Target(currentOrigin, currentTarget);
        }

        void CancelTargeting(InputAction.CallbackContext _)
        {
            InputHandler.Controls.Game.Target.performed -= ExecuteAction;
            InputHandler.Controls.Game.CancelTarget.performed -= CancelTargeting;

            SetTargetingType(defaultTargetingType);

            updateInTargetSelectionMode = true;
        }

        void ExecuteAction(InputAction.CallbackContext _)
        {
            if (!InTargetSelectionMode) return;

            var actionContext = new ActionContext(currentAction, MousePosition);

            if (!currentPlayerController.StartAction(actionContext)) return;

            InputHandler.Controls.Game.Target.performed -= ExecuteAction;
            InputHandler.Controls.Game.CancelTarget.performed -= CancelTargeting;

            SetTargetingType(defaultTargetingType);

            CancelTargeting(_);
        }

        void DrawPath()
        {
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