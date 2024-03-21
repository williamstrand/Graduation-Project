using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public abstract class TargetingType
    {
        public abstract void Target(Vector2Int origin, Vector2Int target, TargetingReticle reticle);
        public abstract void StopTarget();
    }
}