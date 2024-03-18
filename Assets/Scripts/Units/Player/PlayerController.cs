using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Units.Upgrades;

namespace WSP.Units.Player
{
    public class PlayerController : UnitController, IPlayerUnitController
    {
        public Action<int> OnUnitLevelUp { get; set; }
        public Action<float, float> OnUnitXpGained { get; set; }
        public Action<float, float> OnUnitHealthChanged { get; set; }

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
                    targetPosition = Unit.GridPosition;
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
            if (unit == null) return;

            if (Unit != null)
            {
                Unit.OnTargetKilled -= killed => Unit.AddXp(killed.Level * 15);
                Unit.OnLevelUp -= level => OnUnitLevelUp?.Invoke(level);
                Unit.OnXpGained -= (current, max) => OnUnitXpGained?.Invoke(current, max);
                Unit.OnHealthChanged -= (current, max) => OnUnitHealthChanged?.Invoke(current, max);
            }

            base.SetUnit(unit);

            targetPosition = Unit.GridPosition;
            
            Unit.OnTargetKilled += killed => Unit.AddXp(killed.Level * 15);
            Unit.OnLevelUp += level => OnUnitLevelUp?.Invoke(level);
            Unit.OnXpGained += (current, max) => OnUnitXpGained?.Invoke(current, max);
            Unit.OnHealthChanged += (current, max) => OnUnitHealthChanged?.Invoke(current, max);
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

        public void AddUpgrade(IUpgrade upgrade)
        {
            upgrade.Apply(Unit);
        }
    }
}