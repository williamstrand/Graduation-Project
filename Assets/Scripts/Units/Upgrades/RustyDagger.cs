using UnityEngine;
using WSP.Units.Upgrades.UpgradeEffects;

namespace WSP.Units.Upgrades
{
    public class RustyDagger : Upgrade
    {
        public override Sprite Icon => null;
        public override string Description => "Increases attack damage by 10%";

        public RustyDagger()
        {
            var stats = new Stats(1);
            stats.Attack *= 1.1f;
            UpgradeTypes = new IUpgradeEffect[] { new MultiplicativeStatUpgrade(stats) };
        }
    }
}