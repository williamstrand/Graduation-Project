using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Targeting.TargetingTypes
{
    public class LineTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target) { }
        public override void StopTarget() { }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            var distance = Pathfinder.Distance(origin, target);
            var diff = target - origin;
            var direction = new Vector2Int(diff.x == 0 ? 0 : diff.x / Mathf.Abs(diff.x), diff.y == 0 ? 0 : diff.y / Mathf.Abs(diff.y));
            var targets = new Vector2Int[distance];

            for (var i = 0; i < distance; i++)
            {
                targets[i] = origin + direction * i;
            }

            return targets;
        }
    }
}