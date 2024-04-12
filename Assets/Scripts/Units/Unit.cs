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
        public Action<int> OnLevelUp { get; set; }
        public Action<float, float> OnXpGained { get; set; }
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

        IAction currentAction;

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
            currentAction = action.Action;
            currentAction.OnActionFinished += ActionSuccess;
            var success = currentAction.StartAction(this, action.Target);

            if (success) return true;

            currentAction.OnActionFinished -= ActionSuccess;
            currentAction = null;
            return false;
        }

        void ActionSuccess()
        {
            if(currentAction == null) return;
            
            OnActionFinished?.Invoke(currentAction);
            currentAction.OnActionFinished -= ActionSuccess;
            currentAction = null;
        }
    }
}