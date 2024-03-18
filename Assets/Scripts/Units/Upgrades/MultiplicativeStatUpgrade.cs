using UnityEngine;

namespace WSP.Units.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Multiplicative Stat Upgrade")]
    public class MultiplicativeStatUpgrade : StatUpgrade
    {
        public override void Apply(IUnit target)
        {
            target.Stats *= Stats;
        }
    }
}