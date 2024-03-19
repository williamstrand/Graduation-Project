using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Items;
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
        public Action<Item[]> OnOpenInventory { get; set; }

        UnityEngine.Camera mainCamera;

        ActionTarget currentTarget;

        void Start()
        {
            mainCamera = UnityEngine.Camera.main;
        }

        void OnEnable()
        {
            InputHandler.Controls.Menu.Inventory.performed += _ => OpenInventory();
        }

        void OpenInventory()
        {
            OnOpenInventory?.Invoke(Unit.Inventory.GetAllItems());
        }

        void Update()
        {
            var gridPosition = GetTargetPosition();
            TargetAction = GetAction(gridPosition);
            ShowTargeting(gridPosition);

            if (!IsTurn) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!CanAct) return;

            StartAction(TargetAction);
        }

        ActionContext GetAction(Vector2Int targetPosition)
        {
            if (InputHandler.Controls.Game.Stop.triggered)
            {
                currentTarget = null;
                return null;
            }

            if (targetPosition == Unit.GridPosition) return null;

            bool inRange;

            if (InputHandler.Controls.Game.Target.triggered)
            {
                currentTarget = new ActionTarget
                {
                    TargetPosition = targetPosition
                };

                if (!GameManager.CurrentLevel.IsOccupied(targetPosition)) return new ActionContext(Unit.Movement, currentTarget);

                currentTarget.TargetUnit = GameManager.CurrentLevel.GetUnitAt(targetPosition);

                inRange = Pathfinder.Distance(Unit.GridPosition, currentTarget.TargetUnit.GridPosition) <= Unit.Stats.AttackRange;
                return inRange ? new ActionContext(Unit.Attack, currentTarget) : new ActionContext(Unit.Movement, currentTarget);
            }

            if (currentTarget == null) return null;

            if (currentTarget.TargetPosition == Unit.GridPosition)
            {
                currentTarget = null;
                return null;
            }

            if (currentTarget.TargetUnit == null) return new ActionContext(Unit.Movement, currentTarget);

            inRange = Pathfinder.Distance(Unit.GridPosition, currentTarget.TargetUnit.GridPosition) <= Unit.Stats.AttackRange;
            if (!inRange) return new ActionContext(Unit.Movement, currentTarget);

            var ctx = new ActionContext(Unit.Attack, currentTarget);
            currentTarget = null;
            return ctx;
        }

        Vector2Int GetTargetPosition()
        {
            var mousePosition = InputHandler.Controls.Game.MousePosition.ReadValue<Vector2>();
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);

            return gridPosition;
        }

        void ShowTargeting(Vector2Int gridPosition)
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
            currentTarget = null;
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