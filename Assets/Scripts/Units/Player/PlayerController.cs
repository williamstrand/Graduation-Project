using System;
using UnityEngine;
using WSP.Camera;
using WSP.Input;
using WSP.Items;
using WSP.Targeting;
using WSP.Units.SpecialAttacks;
using WSP.Units.Upgrades;

namespace WSP.Units.Player
{
    public class PlayerController : UnitController
    {
        public Action<float, float> OnUnitHealthChanged { get; set; }
        public Action OnOpenInventory { get; set; }

        public TargetingComponent TargetingComponent { get; private set; }

        ActionTarget currentTarget;
        ActionContext targetAction;

        class ActionTarget
        {
            public Unit TargetUnit;
            public Vector2Int TargetPosition;
        }

        void Awake()
        {
            TargetingComponent = GetComponent<TargetingComponent>();
        }

        void Start()
        {
            Unit.SpecialAttack.SetSpecialAttack(0, new Fireball());
            Unit.SpecialAttack.SetSpecialAttack(1, new ArcaneExplosion());
            Unit.SpecialAttack.SetSpecialAttack(2, new Whirlwind());
            Unit.Inventory.AddItem(new BlastGem());
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
            if (Unit.ActionInProgress) return;

            CameraController.SetTargetPosition(Unit.GridPosition);

            if (!StartAction(targetAction)) currentTarget = null;
            targetAction = null;
        }

        void Target(Vector2 position)
        {
            if (TargetingComponent.InTargetSelectionMode) return;

            var gridPosition = GameManager.CurrentLevel.Map.GetGridPosition(position);

            if (gridPosition == Unit.GridPosition) return;
            if (GameManager.CurrentLevel.IsHidden(gridPosition) && !GameManager.CurrentLevel.IsFound(gridPosition)) return;

            targetAction = null;
            currentTarget = new ActionTarget
            {
                TargetPosition = gridPosition
            };

            if (GameManager.CurrentLevel.IsOccupied(gridPosition, out var target))
            {
                if (target.IsVisible) currentTarget.TargetUnit = target;
            }

            GetAction(gridPosition);
        }

        void GetAction(Vector2Int targetPosition)
        {
            if (!IsTurn) return;
            if (TargetingComponent.InTargetSelectionMode) return;
            if (targetPosition == Unit.GridPosition) return;

            if (targetAction != null) return;
            if (currentTarget == null) return;

            if (currentTarget.TargetPosition == Unit.GridPosition)
            {
                Stop();
                return;
            }

            if (currentTarget.TargetUnit == null)
            {
                targetAction = new ActionContext(Unit.Movement, currentTarget.TargetPosition);
                return;
            }

            currentTarget.TargetPosition = currentTarget.TargetUnit.GridPosition;
            var inRange = Unit.Attack.IsInRange(Unit.GridPosition, currentTarget.TargetPosition);
            if (!inRange)
            {
                targetAction = new ActionContext(Unit.Movement, currentTarget.TargetPosition);
                return;
            }

            targetAction = new ActionContext(Unit.Attack, currentTarget.TargetPosition);
            currentTarget = null;
        }

        void Stop()
        {
            currentTarget = null;
            targetAction = null;
        }

        void UseSpecialAttack(int index)
        {
            if (Unit.SpecialAttack.SpecialAttacks[index] == null) return;

            TargetingComponent.StartActionTargeting(Unit.SpecialAttack[index]);
        }

        public override void SetUnit(Unit unit)
        {
            if (unit == null) return;

            if (Unit != null)
            {
                Unit.OnTargetKilled -= UnitTargetKilled;
                Unit.OnHealthChanged -= UnitHealthChanged;
            }

            base.SetUnit(unit);

            Unit.OnTargetKilled += UnitTargetKilled;
            Unit.OnHealthChanged += UnitHealthChanged;
        }

        void UnitTargetKilled(Unit target)
        {
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