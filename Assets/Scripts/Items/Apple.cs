using WSP.Units;

namespace WSP.Items
{
    public class Apple : Item
    {
        const int HealAmount = 10;

        protected override void ActivateEffect(IUnit origin, ActionTarget target)
        {
            origin.Heal(HealAmount);
            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}