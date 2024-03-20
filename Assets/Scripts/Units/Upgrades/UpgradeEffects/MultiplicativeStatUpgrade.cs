namespace WSP.Units.Upgrades.UpgradeEffects
{
    public class MultiplicativeStatUpgrade : StatUpgrade
    {
        public MultiplicativeStatUpgrade(Stats stats) : base(stats) { }

        public override void Apply(IUnit target)
        {
            target.Stats *= Stats;
        }
    }
}