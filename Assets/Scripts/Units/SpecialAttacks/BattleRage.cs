using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units.Buffs;

namespace WSP.Units.SpecialAttacks
{
    public class BattleRage : SpecialAttack
    {
        const float AttackIncrease = 1.5f;

        public override string Name => "Battle Rage";
        public override TargetingType TargetingType { get; } = new SelfTargeting();
        public override string Description => $"Increases attack by {AttackIncrease * 100 - 100}% for {buff.Duration} turns.";
        public override int Cooldown => 5;

        Buff buff = new BattleRageBuff();

        protected override bool ExecuteAction(Unit origin, Vector2Int target)
        {
            ActionInProgress = true;
            origin.StartCoroutine(ApplyBuff(origin));
            return true;
        }

        IEnumerator ApplyBuff(Unit unit)
        {
            yield return new WaitForSeconds(.5f);

            ActionInProgress = false;
            OnTurnOver?.Invoke();

            buff.Apply(unit);
            yield return null;
        }
    }
}