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

        ActionTarget currentTarget;

        class ActionTarget
        {
            public IUnit TargetUnit;
            public Vector2Int TargetPosition;
        }

        void Start()
        {
            Unit.SpecialAttack.SetSpecialAttack(0, new Fireball());
        }

        void OnEnable()
        {
            InputHandler.OnInventory += OpenInventory;
            InputHandler.OnTarget += Target;
            InputHandler.OnStop += Stop;
            InputHandler.OnSpecialAttack += UseSpecialAttack;
        }

        void OnDisable()
        {
            InputHandler.OnInventory -= OpenInventory;
        }

        void OpenInventory()
        {
            OnOpenInventory?.Invoke();
        }

        void Update()
        {
            var gridPosition = TargetingManager.GetTarget();
            GetAction(gridPosition);

            if (!IsTurn) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!CanAct) return;

            StartTargetAction();
        }

        void Target(Vector2 position)
        {
            if (TargetingManager.InTargetSelectionMode) return;

            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(position);

            if (gridPosition == Unit.GridPosition) return;

            GetTarget(gridPosition);
        }

        void GetAction(Vector2Int targetPosition)
        {
            if (TargetingManager.InTargetSelectionMode) return;
            if (targetPosition == Unit.GridPosition) return;

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

        void UseSpecialAttack(int index)
        {
            if (Unit.SpecialAttack.SpecialAttacks[index] == null) return;

            var actionContext = new ActionContext(Unit.SpecialAttack[0], GameManager.CurrentLevel.Map.GetGridPosition(InputHandler.MousePosition));
            TargetAction = actionContext;
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