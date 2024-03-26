namespace WSP.Units.Components
{
    public interface ISpecialAttackComponent
    {
        IAction[] SpecialAttacks { get; }

        IAction this[int index] { get; }
        void SetSpecialAttack(int index, IAction specialAttack);
    }
}