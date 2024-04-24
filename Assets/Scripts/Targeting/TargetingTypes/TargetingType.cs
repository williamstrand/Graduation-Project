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

        protected virtual bool ShouldHide(Vector2Int origin, Vector2Int target)
        {
            var reticle = TargetingComponent.Reticle.Type == TargetingReticle.ReticleTargetType.None;
            var hidden = GameManager.CurrentLevel.IsHidden(target) && !GameManager.CurrentLevel.IsFound(target);
            var inRange = TargetingComponent.CurrentAction?.IsInRange(origin, target) ?? true;
            return reticle || hidden || !inRange;
        }
    }
}