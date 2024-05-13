using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class SelfTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            base.Target(origin, target);

            if (!HasChanged) return;

            ReturnAllReticles();

            var reticle = GetReticle();
            reticle.SetPosition(origin);
            reticle.SetType(TargetingReticle.ReticleTargetType.Friendly);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { origin };
        }
    }
}