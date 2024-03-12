using System;
using UnityEngine;
using WSP.Units.Components;

namespace WSP.Units
{
    [RequireComponent(typeof(IMovementComponent),
        typeof(IAttackComponent))]
    public class Unit : MonoBehaviour, IUnit
    {
        public Action OnDeath { get; set; }
        public Action OnActionFinished { get; set; }
        public Vector2Int GridPosition => movement.GridPosition;
        public int CurrentHealth { get; private set; }
        public GameObject GameObject => gameObject;

        IMovementComponent movement;
        IAttackComponent attack;
        [field: SerializeField] public Stats Stats { get; private set; }

        protected void Awake()
        {
            movement = GetComponent<MovementComponent>();
            attack = GetComponent<AttackComponent>();
        }

        protected void Start()
        {
            attack.OnAttackFinished += ActionFinished;

            CurrentHealth = Stats.Health;
        }

        public bool MoveTo(Vector2 position)
        {
            var gridPos = GameManager.CurrentLevel.Map.GetGridPosition(position);
            return movement.MoveTo(GameManager.CurrentLevel.Map.GetWorldPosition(gridPos));
        }

        public void Attack(IUnit target)
        {
            attack.Attack(this, target);
        }

        public void Damage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth > 0) return;

            Kill();
        }

        void Kill()
        {
            OnDeath?.Invoke();
        }

        void ActionFinished()
        {
            OnActionFinished?.Invoke();
        }
    }
}