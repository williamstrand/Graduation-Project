using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class PositionTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (TargetingManager.Reticle.Type == TargetingReticle.ReticleTargetType.Normal)
            {
                TargetingManager.Reticle.SetPosition(target);
                TargetingManager.Reticle.Enable(true);
                return;
            }

            TargetingManager.Reticle.Enable(false);
        }

        public override void StopTarget() { }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}