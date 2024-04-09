using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    /// <summary>
    /// Hits every cell in a square area around the caster.
    /// </summary>
    public class AuraTargeting : AreaTargeting
    {
        public AuraTargeting(int radius) : base(radius, radius) { }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            TargetingComponent.Reticle.SetPosition(origin);
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
                    var position = origin + new Vector2Int(x, y);

                    if (position == origin) continue;

                    targets[index] = position;
                }
            }

            return targets;
        }
    }
}