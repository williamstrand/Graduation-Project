using System;
using UnityEngine;
using WSP.Map;
using WSP.Units.Components;

namespace WSP.Units
{
    public interface IUnit : ILevelObject
    {
        Action OnDeath { get; set; }
        Action<IUnit> OnTargetKilled { get; set; }
        Action<float, float> OnHealthChanged { get; set; }
        Action<IAction> OnActionFinished { get; set; }

        float CurrentHealth { get; }
        GameObject GameObject { get; }
        Stats Stats { get; }
        bool ActionInProgress { get; }

        IMovementComponent Movement { get; }
        IAttackComponent Attack { get; }
        IInventoryComponent Inventory { get; }
        ISpecialAttackComponent SpecialAttack { get; }

        bool Damage(float damage);
        void Heal(float heal);
        bool StartAction(ActionContext action);
    }
}