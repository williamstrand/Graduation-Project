using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
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

        public TargetingComponent TargetingComponent { get; private set; }

        ActionTarget currentTarget;

        class ActionTarget
        {
            public IUnit TargetUnit;
            public Vector2Int TargetPosition;
        }

        void Awake()
        {
            TargetingComponent = GetComponent<TargetingComponent>();
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
            InputHandler.OnTarget -= Target;
            InputHandler.OnStop -= Stop;
            InputHandler.OnSpecialAttack -= UseSpecialAttack;
        }

        void OpenInventory()
        {
            OnOpenInventory?.Invoke();
        }

        void Update()
        {
            var gridPosition = TargetingComponent.CurrentTarget;
            GetAction(gridPosition);

            if (!IsTurn) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!CanAct) return;

            StartAction(TargetAction);
        }

        void Target(Vector2 position)
        {
            if (TargetingComponent.InTargetSelectionMode) return;

            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(position);

            if (gridPosition == Unit.GridPosition) return;

            TargetAction = null;
            currentTarget = new ActionTarget
            {
                TargetPosition = gridPosition
            };

            if (GameManager.CurrentLevel.IsOccupied(gridPosition))
            {
                currentTarget.TargetUnit = GameManager.CurrentLevel.GetUnitAt(gridPosition);
            }
        }

        void GetAction(Vector2Int targetPosition)
        {
            if (TargetingComponent.InTargetSelectionMode) return;
            if (targetPosition == Unit.GridPosition) return;

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
            Unit.Attack.SetRange(Unit.Stats.AttackRange);
            var inRange = Unit.Attack.IsInRange(Unit.GridPosition, currentTarget.TargetPosition);
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

        void UseSpecialAttack(int index)
        {
            if (Unit.SpecialAttack.SpecialAttacks[index] == null) return;

            TargetingComponent.StartActionTargeting(Unit.SpecialAttack[index]);
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