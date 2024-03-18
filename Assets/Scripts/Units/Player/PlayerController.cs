using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Map.Pathfinding;
using WSP.Targeting;
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

        ActionTarget target;

        void Start()
        {
            mainCamera = UnityEngine.Camera.main;

            controls = new Controls();
            controls.Enable();
        }

        void Update()
        {
            var gridPosition = GetTargetPosition();
            TargetAction = GetAction(gridPosition);
            Target(gridPosition);

            if (!IsTurn) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!CanAct) return;

            StartAction(TargetAction);
        }

        ActionContext GetAction(Vector2Int targetPosition)
        {
            if (controls.Game.Stop.triggered)
            {
                target = null;
                return null;
            }

            bool inRange;
            if (target != null)
            {
                if (target.TargetPosition == Unit.GridPosition)
                {
                    target = null;
                    return null;
                }

                if (target.TargetUnit == null) return new ActionContext(Unit.Movement, target);

                inRange = Pathfinder.Distance(Unit.GridPosition, target.TargetUnit.GridPosition) <= Unit.Stats.AttackRange;
                if (!inRange) return new ActionContext(Unit.Movement, target);

                var ctx = new ActionContext(Unit.Attack, target);
                target = null;
                return ctx;
            }

            if (targetPosition == Unit.GridPosition) return null;
            if (!controls.Game.LeftClick.triggered) return null;

            target = new ActionTarget
            {
                TargetPosition = targetPosition
            };

            if (!GameManager.CurrentLevel.IsOccupied(targetPosition)) return new ActionContext(Unit.Movement, target);

            target.TargetUnit = GameManager.CurrentLevel.GetUnitAt(targetPosition);

            inRange = Pathfinder.Distance(Unit.GridPosition, target.TargetUnit.GridPosition) <= Unit.Stats.AttackRange;
            return inRange ? new ActionContext(Unit.Attack, target) : new ActionContext(Unit.Movement, target);
        }

        Vector2Int GetTargetPosition()
        {
            var mousePosition = controls.Game.MousePosition.ReadValue<Vector2>();
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);

            return gridPosition;
        }

        void Target(Vector2Int gridPosition)
        {
            var type = TargetingReticle.TargetType.Normal;

            if (GameManager.CurrentLevel.IsOccupied(gridPosition))
            {
                var targetUnit = GameManager.CurrentLevel.GetUnitAt(gridPosition);
                type = targetUnit == Unit ? TargetingReticle.TargetType.None : TargetingReticle.TargetType.Enemy;
            }

            TargetingManager.SetTargetPosition(Unit.GridPosition, gridPosition, type);
        }

        public override void SetUnit(IUnit unit)
        {
            if (unit == null) return;

            if (Unit != null)
            {
                Unit.OnTargetKilled -= UnitTargetKilled;
                Unit.OnLevelUp -= UnitLevelUp;
                Unit.OnXpGained -= UnitXpGained;
                Unit.OnHealthChanged -= UnitHealthChanged;
            }

            base.SetUnit(unit);

            Unit.OnTargetKilled += UnitTargetKilled;
            Unit.OnLevelUp += UnitLevelUp;
            Unit.OnXpGained += UnitXpGained;
            Unit.OnHealthChanged += UnitHealthChanged;
        }

        void UnitXpGained(float current, float max)
        {
            OnUnitXpGained?.Invoke(current, max);
        }

        void UnitLevelUp(int level)
        {
            OnUnitLevelUp?.Invoke(level);
        }

        void UnitTargetKilled(IUnit target)
        {
            Unit.AddXp(target.Level * 15);
            this.target = null;
            Debug.Log("Target killed");
        }

        void UnitHealthChanged(float current, float max)
        {
            OnUnitHealthChanged?.Invoke(current, max);
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