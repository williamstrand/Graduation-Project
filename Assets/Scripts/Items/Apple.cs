using UnityEngine;
using WSP.Units;

namespace WSP.Items
{
    public class Apple : Item
    {
        protected virtual int HealAmount => 25;
        public override int Weight => 75;
        public override Sprite Icon => null;
        public override TargetingType TargetingType => TargetingType.Self;

        protected override void ActivateEffect(IUnit origin, ActionTarget target)
        {
            origin.Heal(HealAmount);
            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}