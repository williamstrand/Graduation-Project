using WSP.Units;

namespace WSP.Items
{
    public class Apple : Item
    {
        protected virtual int HealAmount => 25;
        public override int Weight => 75;
        public override TargetingType TargetingType => TargetingType.Self;

        protected override bool ActivateEffect(IUnit origin, ActionTarget target)
        {
            origin.Heal(HealAmount);
            ActionStarted = false;
            OnActionFinished?.Invoke();
            return true;
        }
    }
}