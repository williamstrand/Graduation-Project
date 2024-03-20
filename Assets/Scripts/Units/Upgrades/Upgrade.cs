using UnityEngine;
using WSP.Units.Upgrades.UpgradeEffects;

namespace WSP.Units.Upgrades
{
    public abstract class Upgrade : IUpgrade
    {
        public abstract Sprite Icon { get; }
        public virtual string Name => GetType().Name;
        public abstract string Description { get; }

        protected IUpgradeEffect[] UpgradeTypes { get; set; }

        public void Apply(IUnit target)
        {
            for (var i = 0; i < UpgradeTypes.Length; i++)
            {
                UpgradeTypes[i].Apply(target);
            }
        }
    }
}