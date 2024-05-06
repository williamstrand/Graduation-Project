using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class SelfTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            TargetingComponent.Reticle.SetPosition(origin);
            TargetingComponent.Reticle.SetColor(TargetingComponent.FriendlyColor);
            TargetingComponent.Reticle.Enable(true);
        }

        public override void StopTarget()
        {
            TargetingComponent.Reticle.SetColor(TargetingComponent.NormalColor);
            TargetingComponent.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { origin };
        }
    }
}