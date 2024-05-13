using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace WSP.Targeting.TargetingTypes
{
    public abstract class TargetingType
    {
        protected TargetingComponent TargetingComponent;

        static Queue<TargetingReticle> reticlePool = new();
        static List<TargetingReticle> reticles = new();

        static AssetLoader<TargetingReticle> reticleLoader = new("level");

        Vector2Int lastOrigin;
        Vector2Int lastTarget;

        protected bool HasChanged { get; private set; }

        public void StartTarget(TargetingComponent targetingComponent)
        {
            TargetingComponent = targetingComponent;
        }

        public virtual void Target(Vector2Int origin, Vector2Int target)
        {
            if (origin == lastOrigin && target == lastTarget)
            {
                HasChanged = false;
                return;
            }

            lastOrigin = origin;
            lastTarget = target;
            HasChanged = true;
        }

        public void StopTarget()
        {
            ReturnAllReticles();
            TargetingComponent.HidePath();
        }

        public abstract Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target);

        protected virtual bool ShouldHide(Vector2Int origin, Vector2Int target)
        {
            var hidden = GameManager.CurrentLevel.IsHidden(target) && !GameManager.CurrentLevel.IsFound(target);
            var inRange = TargetingComponent.CurrentAction?.IsInRange(origin, target) ?? true;
            var isWall = GameManager.CurrentLevel.Map.GetValue(target) == Map.Pathfinding.Map.Wall;
            return isWall || hidden || !inRange;
        }

        protected TargetingReticle GetReticle()
        {
            reticlePool ??= new Queue<TargetingReticle>();

            var reticle = reticlePool.Count == 0 ? Object.Instantiate(reticleLoader.LoadAsset("Target Reticle"), TargetingComponent.transform) : reticlePool.Dequeue();
            reticles.Add(reticle);
            reticle.Enable(true);
            reticle.Reset();
            return reticle;
        }

        static void ReturnReticle(TargetingReticle reticle)
        {
            if (reticle == null) return;

            reticles.Remove(reticle);
            reticle.Enable(false);
            reticlePool.Enqueue(reticle);
        }

        protected static void ReturnAllReticles()
        {
            if (reticles == null) return;

            var reticlesCopy = new List<TargetingReticle>(reticles);

            foreach (var reticle in reticlesCopy)
            {
                ReturnReticle(reticle);
            }
        }
    }
}