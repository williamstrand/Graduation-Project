using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class UnitTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target, TargetingReticle reticle)
        {
            if (reticle.Type == TargetingReticle.ReticleTargetType.Enemy)
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