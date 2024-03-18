using System;

namespace WSP.Units.Components
{
    public interface IAttackComponent
    {
        Action OnAttackFinished { get; set; }
        Action<IUnit, bool> OnAttackHit { get; set; }

        void Attack(IUnit attacker, IUnit target);
    }
}