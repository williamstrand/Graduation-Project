using UnityEngine;

namespace WSP.Units.Upgrades
{
    public interface IUpgrade
    {
        Sprite Icon { get; }
        string Name { get; }
        string Description { get; }

        void Apply(Unit target);
    }
}