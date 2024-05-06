using WSP.Items;
using WSP.Units.Components;
using WSP.Units.SpecialAttacks;

namespace WSP.Units.Buffs
{
    public class BattleRageBuff : Buff
    {
        const float AttackIncrease = 1.5f;

        public override string Name => "Battle Rage";
        public override string Description => $"Increases attack by {AttackIncrease * 100 - 100}% for {Duration} attacks.";
        public override int Duration => 3;

        public override void Apply(Unit unit)
        {
            base.Apply(unit);

            unit.Stats.Attack *= AttackIncrease;
            unit.OnActionFinished += Tick;
        }

        public override void Remove(Unit unit)
        {
            base.Remove(unit);

            unit.Stats.Attack /= AttackIncrease;
            unit.OnActionFinished -= Tick;
        }

        protected override void Tick(IAction action, Unit unit)
        {
            if (action is not SpecialAttack or AttackComponent or Item) return;

            base.Tick(action, unit);
        }
    }
}