using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units;

namespace WSP.Items
{
    public class Apple : Item
    {
        const float Duration = 1.5f;

        protected virtual int HealAmount => 25;
        public override int Weight => 75;
        public override TargetingType TargetingType => new SelfTargeting();

        protected override bool ActivateEffect(IUnit origin, Vector2Int target)
        {
            origin.Heal(HealAmount);
            ActionInProgress = false;
            OnTurnOver?.Invoke();
            return true;
        }
    }
}