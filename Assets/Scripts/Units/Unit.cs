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
        public Action<IUnit> OnTargetKilled { get; set; }
        public Action OnActionFinished { get; set; }
        public Action<int> OnLevelUp { get; set; }
        public Action<float, float> OnXpGained { get; set; }
        public Action<float, float> OnHealthChanged { get; set; }

        public Vector2Int GridPosition => movement.GridPosition;
        public int Level { get; private set; } = 1;
        public float CurrentHealth { get; private set; }
        public GameObject GameObject => gameObject;
        [field: SerializeField] public Stats Stats { get; set; }
        public float Xp { get; private set; }
        public float XpToNextLevel => 100 + Level * 50;

        IMovementComponent movement;
        IAttackComponent attack;

        protected void Awake()
        {
            movement = GetComponent<MovementComponent>();
            attack = GetComponent<AttackComponent>();

            CurrentHealth = Mathf.RoundToInt(Stats.Health);
            attack.OnAttackFinished += ActionFinished;
            attack.OnAttackHit += TargetHit;
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

        public bool Damage(float damage)
        {
            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(CurrentHealth, Stats.Health);

            if (CurrentHealth > 0) return false;

            Kill();
            return true;
        }

        public void AddXp(int xp) { }

        void Kill()
        {
            OnDeath?.Invoke();
        }

        void ActionFinished()
        {
            OnActionFinished?.Invoke();
        }

        void TargetHit(IUnit target, bool killed)
        {
            if (killed) OnTargetKilled?.Invoke(target);
        }

        void LevelUp()
        {
            Level++;
            OnLevelUp?.Invoke(Level);
        }

        public void AddXp(float amount)
        {
            Xp += amount;
            if (Xp >= XpToNextLevel)
            {
                var extraXp = Xp - XpToNextLevel;
                Xp = 0;
                LevelUp();
                AddXp(extraXp);
            }

            OnXpGained?.Invoke(Xp, XpToNextLevel);
        }
    }
}