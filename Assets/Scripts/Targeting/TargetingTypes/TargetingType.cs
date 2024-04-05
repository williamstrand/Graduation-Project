using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public abstract class TargetingType
    {
        protected TargetingComponent TargetingComponent;

        public virtual void StartTarget(TargetingComponent targetingComponent)
        {
            TargetingComponent = targetingComponent;
        }

        public abstract void Target(Vector2Int origin, Vector2Int target);
        public virtual void StopTarget() { }
        public abstract Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target);
    }
}