using WSP.Targeting.TargetingTypes;
using WSP.Units;

namespace WSP.Items
{
    public class BlastGem : Item
    {
        const int Width = 3;
        const int Height = 3;

        public override string Name => "Blast Gem";
        public override string Description => "Triggers an explosion.";
        public override int Weight => 30;
        public override TargetingType TargetingType { get; } = new AreaTargeting(Width, Height);

        protected override bool ActivateEffect(IUnit origin, ActionTarget target)
        {
            OnActionFinished?.Invoke();
            return true;
        }
    }
}