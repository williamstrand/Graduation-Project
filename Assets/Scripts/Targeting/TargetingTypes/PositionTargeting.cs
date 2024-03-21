using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class PositionTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target, TargetingReticle reticle)
        {
            if (reticle.Type == TargetingReticle.ReticleTargetType.Normal)
            {
                reticle.SetPosition(target);
                reticle.Enable(true);
                return;
            }

            reticle.Enable(false);
        }

        public override void StopTarget() { }
    }
}