using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class UnitTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (TargetingManager.Reticle.Type == TargetingReticle.ReticleTargetType.Enemy)
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