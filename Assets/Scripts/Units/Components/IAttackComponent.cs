using System;

namespace WSP.Units.Components
{
    public interface IAttackComponent
    {
        Action OnAttackFinished { get; set; }

        void Attack(IUnit attacker, IUnit target);
    }
}