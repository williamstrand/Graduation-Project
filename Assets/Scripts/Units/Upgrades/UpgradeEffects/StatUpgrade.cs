namespace WSP.Units.Upgrades.UpgradeEffects
{
    public abstract class StatUpgrade : IUpgradeEffect
    {
        public StatUpgrade(Stats stats)
        {
            Stats = stats;
        }

        protected Stats Stats { get; set; }
        public abstract void Apply(IUnit target);
    }
}