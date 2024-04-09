using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class AreaTargeting : TargetingType
    {
        protected int Width;
        protected int Height;

        public AreaTargeting(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override void StartTarget(TargetingComponent targetingComponent)
        {
            base.StartTarget(targetingComponent);
            TargetingComponent.Reticle.SetSize(new Vector2(Width * GameManager.CurrentLevel.Map.CellSize, Height * GameManager.CurrentLevel.Map.CellSize));
            TargetingComponent.Reticle.Enable(true);
        }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (!TargetingComponent.CurrentAction.IsInRange(origin, target))
            {
                TargetingComponent.Reticle.Enable(false);
                return;
            }

            TargetingComponent.Reticle.Enable(true);
            TargetingComponent.Reticle.SetPosition(target);
        }

        public override void StopTarget()
        {
            TargetingComponent.Reticle.SetSize(new Vector2(1, 1));
            TargetingComponent.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            var index = 0;
            var targets = new Vector2Int[Width * Height];
            for (var x = -Width / 2; x <= Width / 2; x++)
            {
                for (var y = -Height / 2; y <= Height / 2; y++, index++)
                {
                    targets[index] = target + new Vector2Int(x, y);
                }
            }

            return targets;
        }
    }
}