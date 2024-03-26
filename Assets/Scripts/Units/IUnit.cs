using System;
using UnityEngine;
using WSP.Units.Components;

namespace WSP.Units
{
    public interface IUnit
    {
        Action OnDeath { get; set; }
        Action<IUnit> OnTargetKilled { get; set; }
        Action<int> OnLevelUp { get; set; }
        Action<float, float> OnXpGained { get; set; }
        Action<float, float> OnHealthChanged { get; set; }

        Vector2Int GridPosition { get; }
        float CurrentHealth { get; }
        GameObject GameObject { get; }
        Stats Stats { get; set; }
        Stats StatsPerLevel { get; set; }
        int Level { get; }
        float Xp { get; }
        float XpToNextLevel { get; }

        IMovementComponent Movement { get; }
        IAttackComponent Attack { get; }
        IInventoryComponent Inventory { get; }
        ISpecialAttackComponent SpecialAttack { get; }

        bool Damage(float damage);
        void Heal(float heal);
        void AddXp(float xp);
    }
}