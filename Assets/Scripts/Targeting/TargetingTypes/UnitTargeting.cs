using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class UnitTargeting : PositionTargeting
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            base.Target(origin, target);

            if (!HasChanged) return;

            ReturnAllReticles();

            if (Reticle.Type == TargetingReticle.ReticleTargetType.Enemy) return;

            StopTarget();
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}