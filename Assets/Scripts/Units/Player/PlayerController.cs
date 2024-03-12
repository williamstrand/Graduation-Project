using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Map.Pathfinding;

namespace WSP.Units.Player
{
    public class PlayerController : MonoBehaviour, IUnitController
    {
        public static Vector2Int GridPosition => instance.Unit.GridPosition;
        static PlayerController instance;

        public Action OnTurnEnd { get; set; }
        public IUnit Unit { get; private set; }
        public bool IsTurn { get; set; }

        bool actionStarted;

        Controls controls;
        UnityEngine.Camera mainCamera;

        Vector2Int targetPosition;
        IUnit targetUnit;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            mainCamera = UnityEngine.Camera.main;

            controls = new Controls();
            controls.Enable();

            targetPosition = Unit.GridPosition;
            CameraController.SetTargetPosition(Unit.GridPosition);
        }

        void Update()
        {
            if (controls.Game.Stop.triggered)
            {
                targetPosition = Unit.GridPosition;
                return;
            }

            if (!IsTurn) return;

            if (controls.Game.LeftClick.triggered)
            {
                var mousePosition = controls.Game.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);
                targetPosition = gridPosition;

                if (actionStarted) return;

                targetUnit = GameManager.CurrentLevel.IsOccupied(gridPosition) ? GameManager.CurrentLevel.GetUnitAt(gridPosition) : null;
            }

            if (actionStarted) return;

            if (targetUnit != null)
            {
                if (Pathfinder.Distance(Unit.GridPosition, targetUnit.GridPosition) <= Unit.Stats.AttackRange)
                {
                    Unit.Attack(targetUnit);
                    targetUnit = null;
                    actionStarted = true;
                    return;
                }

                targetPosition = targetUnit.GridPosition;
            }

            if (Unit.MoveTo(GameManager.CurrentLevel.Map.GetWorldPosition(targetPosition)))
            {
                actionStarted = true;
            }
        }

        public void SetUnit(IUnit unit)
        {
            if (Unit != null)
            {
                Unit.OnActionFinished -= EndTurn;
                Unit.OnDeath -= Kill;
            }

            Unit = unit;
            Unit.OnActionFinished += EndTurn;
            Unit.OnDeath += Kill;
            targetPosition = Unit.GridPosition;
        }

        public void TurnStart()
        {
            CameraController.SetTargetPosition(Unit.GridPosition);
            actionStarted = false;
        }

        void EndTurn()
        {
            if (!IsTurn) return;

            OnTurnEnd?.Invoke();
        }

        void Kill()
        {
            Debug.LogError("Player died");
        }
    }
}