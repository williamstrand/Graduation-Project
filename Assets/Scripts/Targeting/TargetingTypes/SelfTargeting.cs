using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class SelfTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target) { }
        public override void StopTarget() { }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { origin };
        }
    }
}