using UnityEngine;

namespace WSP.Units.Upgrades
{
    public abstract class StatUpgrade : Upgrade
    {
        [field: SerializeField] public Stats Stats { get; private set; }
    }
}