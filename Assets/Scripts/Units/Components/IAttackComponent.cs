using System;

namespace WSP.Units.Components
{
    public interface IAttackComponent : IAction
    {
        Action<IUnit, bool> OnAttackHit { get; set; }
    }
}