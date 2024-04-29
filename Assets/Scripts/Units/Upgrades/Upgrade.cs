using UnityEngine;

namespace WSP.Units.Upgrades
{
    public abstract class Upgrade : IUpgrade
    {
        public abstract Sprite Icon { get; }
        public virtual string Name => GetType().Name;
        public abstract string Description { get; }

        public abstract void Apply(Unit target);
    }
}