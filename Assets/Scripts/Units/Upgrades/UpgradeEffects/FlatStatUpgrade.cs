namespace WSP.Units.Upgrades.UpgradeEffects
{
    public class FlatStatUpgrade : StatUpgrade
    {
        public FlatStatUpgrade(Stats stats) : base(stats) { }

        public override void Apply(IUnit target)
        {
            target.Stats += Stats;
        }
    }
}