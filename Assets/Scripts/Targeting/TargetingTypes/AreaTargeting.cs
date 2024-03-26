using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class AreaTargeting : TargetingType
    {
        int width;
        int height;

        public AreaTargeting(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void StartTarget(TargetingManager targetingManager)
        {
            base.StartTarget(targetingManager);
            TargetingManager.Reticle.SetSize(new Vector2(width * GameManager.CurrentLevel.Map.CellSize, height * GameManager.CurrentLevel.Map.CellSize));
            TargetingManager.Reticle.Enable(true);
        }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            TargetingManager.Reticle.SetPosition(target);
        }

        public override void StopTarget()
        {
            TargetingManager.Reticle.SetSize(new Vector2(1, 1));
            TargetingManager.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            var index = 0;
            var targets = new Vector2Int[width * height];
            for (var x = -width / 2; x <= width / 2; x++)
            {
                for (var y = -height / 2; y <= height / 2; y++, index++)
                {
                    targets[index] = target + new Vector2Int(x, y);
                }
            }

            return targets;
        }
    }
}