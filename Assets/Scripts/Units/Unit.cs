using System;
using UnityEngine;
using WSP.Units.Components;

namespace WSP.Units
{
    [RequireComponent(typeof(MovementComponent), typeof(AttackComponent))]
    [RequireComponent(typeof(InventoryComponent), typeof(SpecialAttackComponent))]
    public class Unit : MonoBehaviour, IUnit
    {
        public Action OnDeath { get; set; }
        public Action<IUnit> OnTargetKilled { get; set; }
        public Action<float, float> OnHealthChanged { get; set; }
        public Action<IAction> OnActionFinished { get; set; }
        public Action<Vector2Int> OnMove { get; set; }

        public Vector2Int GridPosition => Movement.GridPosition;
        public float CurrentHealth { get; private set; }
        public GameObject GameObject => gameObject;
        [field: SerializeField] public Stats Stats { get; private set; }

        public MovementComponent Movement { get; private set; }
        public AttackComponent Attack { get; private set; }
        public InventoryComponent Inventory { get; private set; }
        public SpecialAttackComponent SpecialAttack { get; private set; }

        IAction currentAction;
        public bool ActionInProgress => currentAction?.ActionInProgress ?? false;

        SpriteRenderer spriteRenderer;
        public bool IsVisible { get; private set; } = true;

        protected void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Movement = GetComponent<MovementComponent>();
            Movement.OnTurnOver += () => OnMove?.Invoke(Movement.GridPosition);
            Attack = GetComponent<AttackComponent>();
            Inventory = GetComponent<InventoryComponent>();
            SpecialAttack = GetComponent<SpecialAttackComponent>();

            CurrentHealth = Mathf.RoundToInt(Stats.Health);

            Attack.Stats = Stats;
            Movement.Stats = Stats;
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

        public bool StartAction(ActionContext action)
        {
            if (!action.Action.IsInRange(GridPosition, action.Target)) return false;
            if (ActionInProgress) return false;

            currentAction = action.Action;
            currentAction.OnTurnOver += ActionSuccess;
            var success = currentAction.StartAction(this, action.Target, IsVisible);

            if (success) return true;

            currentAction.OnTurnOver -= ActionSuccess;
            return false;
        }

        void ActionSuccess()
        {
            if (currentAction == null) return;

            OnActionFinished?.Invoke(currentAction);
            currentAction.OnTurnOver -= ActionSuccess;
        }

        public void SetVisibility(bool visible)
        {
            spriteRenderer.enabled = visible;
            IsVisible = visible;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}