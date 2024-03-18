using UnityEngine;

namespace WSP.Units.Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Flat Stat Upgrade")]
    public class FlatStatUpgrade : StatUpgrade
    {
        public override void Apply(IUnit target)
        {
            target.Stats += Stats;
        }
    }
}