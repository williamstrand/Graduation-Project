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
            base.Target(origin, target);

            if (!HasChanged) return;

            ReturnAllReticles();

            if (ShouldHide(origin, target))
            {
                StopTarget();
                return;
            }

            var targets = GetTargets(origin, target);

            for (var i = 1; i < targets.Length; i++)
            {
                var reticle = GetReticle();
                reticle.SetPosition(targets[i]);
            }
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