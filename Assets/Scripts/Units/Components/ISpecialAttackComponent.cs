using System;

namespace WSP.Units.Components
{
    public interface ISpecialAttackComponent
    {
        public Action<IAction[]> OnSpecialAttacksChanged { get; set; }

        IAction[] SpecialAttacks { get; }

        IAction this[int index] { get; }
        void SetSpecialAttack(int index, IAction specialAttack);
    }
}