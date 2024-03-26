using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Map.Pathfinding;
using WSP.Targeting;
using WSP.Units.SpecialAttacks;
using WSP.Units.Upgrades;

namespace WSP.Units.Player
{
    public class PlayerController : UnitController, IPlayerUnitController
    {
        public Action<int> OnUnitLevelUp { get; set; }
        public Action<float, float> OnUnitXpGained { get; set; }
        public Action<float, float> OnUnitHealthChanged { get; set; }
        public Action OnOpenInventory { get; set; }

        UnityEngine.Camera mainCamera;

        ActionTarget currentTarget;

        class ActionTarget
        {
            public IUnit TargetUnit;
            public Vector2Int TargetPosition;
        }

        void Start()
        {
            mainCamera = UnityEngine.Camera.main;
            Unit.SpecialAttack.SetSpecialAttack(0, new Fireball());
        }

        void OnEnable()
        {
            InputHandler.Controls.Menu.Inventory.performed += _ => OpenInventory();
        }

        void OpenInventory()
        {
            OnOpenInventory?.Invoke();
        }

        void Update()
        {
            var gridPosition = GetTargetPosition();
            GetAction(gridPosition);
            TargetingManager.Target(Unit.GridPosition, gridPosition);

            if (!IsTurn) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!CanAct) return;

            StartTargetAction();
        }

        void GetAction(Vector2Int targetPosition)
        {
            if (TargetingManager.InTargetSelectionMode) return;

            if (InputHandler.Controls.Game.Stop.triggered)
            {
                Stop();
                return;
            }

            if (targetPosition == Unit.GridPosition) return;

            // Gets target position and unit if the target button is pressed.
            if (InputHandler.Controls.Game.Target.triggered)
            {
                GetTarget(targetPosition);
            }

            if (InputHandler.Controls.Game.Special1.triggered)
            {
                var actionContext = new ActionContext(Unit.SpecialAttack[0], targetPosition);
                TargetAction = actionContext;
            }

            // If there is a target, check if the target is in range and set the action accordingly.
            if (TargetAction != null) return;
            if (currentTarget == null) return;

            if (currentTarget.TargetPosition == Unit.GridPosition)
            {
                Stop();
                return;
            }

            if (currentTarget.TargetUnit == null)
            {
                TargetAction = new ActionContext(Unit.Movement, currentTarget.TargetPosition);
                return;
            }

            currentTarget.TargetPosition = currentTarget.TargetUnit.GridPosition;
            var inRange = Pathfinder.Distance(Unit.GridPosition, currentTarget.TargetUnit.GridPosition) <= Unit.Stats.AttackRange;
            if (!inRange)
            {
                TargetAction = new ActionContext(Unit.Movement, currentTarget.TargetPosition);
                return;
            }

            TargetAction = new ActionContext(Unit.Attack, currentTarget.TargetPosition);
            currentTarget = null;
        }

        void Stop()
        {
            currentTarget = null;
            TargetAction = null;
        }

        void GetTarget(Vector2Int targetPosition)
        {
            TargetAction = null;
            currentTarget = new ActionTarget
            {
                TargetPosition = targetPosition
            };

            if (GameManager.CurrentLevel.IsOccupied(targetPosition))
            {
                currentTarget.TargetUnit = GameManager.CurrentLevel.GetUnitAt(targetPosition);
            }
        }

        Vector2Int GetTargetPosition()
        {
            var mousePosition = InputHandler.Controls.Game.MousePosition.ReadValue<Vector2>();
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(worldPosition);

            return gridPosition;
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