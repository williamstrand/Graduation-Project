using System;
using UnityEngine;
using WSP.Input;

namespace WSP.Units.Player
{
    public class PlayerController : MonoBehaviour, IUnitController
    {
        public Action OnTurnEnd { get; set; }
        public Unit Unit { get; private set; }
        public bool IsTurn { get; set; }

        Controls controls;
        Camera mainCamera;

        Vector2Int targetPosition;

        void Start()
        {
            mainCamera = Camera.main;

            controls = new Controls();
            controls.Enable();

            targetPosition = Unit.GridPosition;
        }

        void Update()
        {
            if (controls.Game.LeftClick.triggered)
            {
                var mousePosition = controls.Game.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var gridPos = GameManager.CurrentMap.GetGridPosition(worldPosition);
                if (GameManager.CurrentMap.GetValue(gridPos) != Map.Map.Wall)
                {
                    targetPosition = gridPos;
                }
            }

            if (controls.Game.Stop.triggered)
            {
                targetPosition = Unit.GridPosition;
            }

            if (!IsTurn) return;
            if (targetPosition == Unit.GridPosition) return;

            Unit.MoveTo(GameManager.CurrentMap.GetWorldPosition(targetPosition));
        }

        public void SetUnit(Unit unit)
        {
            if (Unit != null)
            {
                Unit.OnActionFinished -= EndTurn;
            }

            Unit = unit;
            Unit.OnActionFinished += EndTurn;
            targetPosition = Unit.GridPosition;
        }

        public void TurnStart() { }

        void EndTurn()
        {
            if (!IsTurn) return;

            OnTurnEnd?.Invoke();
        }
    }
}