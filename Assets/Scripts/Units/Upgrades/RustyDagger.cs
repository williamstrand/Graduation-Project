using UnityEngine;

namespace WSP.Units.Upgrades
{
    public class RustyDagger : Upgrade
    {
        public override Sprite Icon => null;
        public override string Description => "Increases attack damage by 10%";

        public override void Apply(Unit target)
        {
            target.Stats.Attack *= 1.1f;
        }
    }
}