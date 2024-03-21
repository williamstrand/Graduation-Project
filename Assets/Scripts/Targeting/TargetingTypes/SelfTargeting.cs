using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class SelfTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target, TargetingReticle reticle) { }
        public override void StopTarget() { }
    }
}