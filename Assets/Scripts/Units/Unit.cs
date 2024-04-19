using System;
using UnityEngine;
using WSP.Units.Components;

namespace WSP.Units
{
    [RequireComponent(typeof(IMovementComponent), typeof(IAttackComponent))]
    [RequireComponent(typeof(IInventoryComponent), typeof(ISpecialAttackComponent))]
    public class Unit : MonoBehaviour, IUnit
    {
        public Action OnDeath { get; set; }
        public Action<IUnit> OnTargetKilled { get; set; }
        public Action<float, float> OnHealthChanged { get; set; }
        public Action<IAction> OnActionFinished { get; set; }

        public Vector2Int GridPosition => Movement.GridPosition;
        public float CurrentHealth { get; private set; }
        public GameObject GameObject => gameObject;
        [field: SerializeField] public Stats Stats { get; private set; }

        public IMovementComponent Movement { get; private set; }
        public IAttackComponent Attack { get; private set; }
        public IInventoryComponent Inventory { get; private set; }
        public ISpecialAttackComponent SpecialAttack { get; private set; }

        public IAction CurrentAction { get; private set; }
        public bool ActionInProgress => CurrentAction?.ActionInProgress ?? false;

        protected void Awake()
        {
            Movement = GetComponent<IMovementComponent>();
            Attack = GetComponent<IAttackComponent>();
            Inventory = GetComponent<IInventoryComponent>();
            SpecialAttack = GetComponent<ISpecialAttackComponent>();

            CurrentHealth = Mathf.RoundToInt(Stats.Health);
            Attack.OnAttackHit += TargetHit;

            Attack.SetRange(Stats.AttackRange);
        }

        public bool Damage(float damage)
        {
            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(CurrentHealth, Stats.Health);

            if (CurrentHealth > 0) return false;

            Kill();
            return true;
        }

        public void Heal(float heal)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + heal, 0, Stats.Health);
            OnHealthChanged?.Invoke(CurrentHealth, Stats.Health);
        }

        void Kill()
        {
            OnDeath?.Invoke();
        }

        void TargetHit(IUnit target, bool killed)
        {
            if (killed) OnTargetKilled?.Invoke(target);
        }

        public bool StartAction(ActionContext action)
        {
            if (!action.Action.IsInRange(GridPosition, action.Target)) return false;
            if (action.Action.ActionInProgress) return false;
            if (CurrentAction is { ActionInProgress: true }) return false;

            CurrentAction = action.Action;
            CurrentAction.OnTurnOver += ActionSuccess;
            var success = CurrentAction.StartAction(this, action.Target);

            if (success) return true;

            CurrentAction.OnTurnOver = null;
            return false;
        }

        void ActionSuccess()
        {
            if (CurrentAction == null) return;

            OnActionFinished?.Invoke(CurrentAction);
            CurrentAction.OnTurnOver -= ActionSuccess;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}