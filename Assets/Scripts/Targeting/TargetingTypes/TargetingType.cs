using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public abstract class TargetingType
    {
        protected TargetingManager TargetingManager;

        public virtual void StartTarget(TargetingManager targetingManager)
        {
            TargetingManager = targetingManager;
        }

        public abstract void Target(Vector2Int origin, Vector2Int target);
        public virtual void StopTarget() { }
        public abstract Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target);
    }
}