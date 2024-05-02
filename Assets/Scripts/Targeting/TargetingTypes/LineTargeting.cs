using System.Collections.Generic;
using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class LineTargeting : TargetingType
    {
        int range;

        public LineTargeting(int range)
        {
            this.range = range;
        }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            TargetingComponent.Reticle.Enable(true);
            var targets = GetTargets(origin, target);
            var length = targets.Length;
            var diff = target - origin;
            var direction = new Vector2Int(diff.x == 0 ? 0 : diff.x / Mathf.Abs(diff.x), diff.y == 0 ? 0 : diff.y / Mathf.Abs(diff.y));

            var size = direction * new Vector2Int(length, length);

            if (size.x == 0)
            {
                size.x = 1;
            }

            if (size.y == 0)
            {
                size.y = 1;
            }

            TargetingComponent.Reticle.SetSize(size);
            TargetingComponent.Reticle.SetPosition(targets[length / 2]);
        }

        public override void StopTarget()
        {
            TargetingComponent.Reticle.SetSize(new Vector2(1, 1));
            TargetingComponent.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            var diff = target - origin;
            var direction = new Vector2Int(diff.x == 0 ? 0 : diff.x / Mathf.Abs(diff.x), diff.y == 0 ? 0 : diff.y / Mathf.Abs(diff.y));
            var targetList = new List<Vector2Int>();

            for (var i = 0; i < range; i++)
            {
                var position = origin + direction * i;
                var isWall = GameManager.CurrentLevel.Map.GetValue(position) == Map.Pathfinding.Map.Wall;
                if (isWall) break;

                targetList.Add(position);
            }

            var targets = targetList.ToArray();
            return targets;
        }
    }
}