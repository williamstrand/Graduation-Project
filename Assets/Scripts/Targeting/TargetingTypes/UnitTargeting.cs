using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class UnitTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (TargetingComponent.Reticle.Type == TargetingReticle.ReticleTargetType.Enemy)
            {
                TargetingComponent.Reticle.SetPosition(target);
                TargetingComponent.Reticle.Enable(true);
                return;
            }

            TargetingComponent.Reticle.Enable(false);
        }

        public override void StopTarget() { }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}