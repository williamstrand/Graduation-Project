﻿using UnityEngine;
using WSP.Camera;
using WSP.Input;

namespace WSP.Units.Player
{
    public class PlayerController : UnitController
    {
        Controls controls;
        UnityEngine.Camera mainCamera;

        Vector2Int targetPosition;
        IUnit targetUnit;

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

            if (controls.Game.LeftClick.triggered)
            {
                var mousePosition = controls.Game.MousePosition.ReadValue<Vector2>();
                var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);
                targetPosition = gridPosition;

                if (ActionStarted) return;

                targetUnit = GameManager.CurrentLevel.IsOccupied(gridPosition) ? GameManager.CurrentLevel.GetUnitAt(gridPosition) : null;
            }

            if (!IsTurn) return;
            if (ActionStarted) return;

            if (targetUnit != null)
            {
                if (Attack(targetUnit))
                {
                    targetUnit = null;
                    return;
                }

                targetPosition = targetUnit.GridPosition;
            }

            if (Unit.MoveTo(GameManager.CurrentLevel.Map.GetWorldPosition(targetPosition)))
            {
                EndTurn();
            }
        }

        public override void SetUnit(IUnit unit)
        {
            base.SetUnit(unit);

            targetPosition = Unit.GridPosition;
        }


        public override void TurnStart()
        {
            base.TurnStart();

            CameraController.SetTargetPosition(Unit.GridPosition);
        }

        protected override void Kill()
        {
            Debug.LogError("Player died");
        }
    }
}