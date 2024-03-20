using WSP.Units;

namespace WSP.Items
{
    public class SwapGem : Item
    {
        public override string Name => "Swap Gem";
        public override string Description => "Swap position with target.";
        public override int Weight => 30;

        protected override void ActivateEffect(IUnit origin, ActionTarget target) { }
    }
}