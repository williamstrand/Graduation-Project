using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class UnitTargeting : TargetingType
    {
        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (ShouldHide(origin, target))
            {
                StopTarget();
                return;
            }

            if (TargetingComponent.Reticle.Type == TargetingReticle.ReticleTargetType.Enemy)
            {
                TargetingComponent.Reticle.SetPosition(target);
                TargetingComponent.Reticle.SetColor(TargetingComponent.EnemyColor);
                TargetingComponent.Reticle.Enable(true);
                return;
            }

            StopTarget();
        }

        public override void StopTarget()
        {
            TargetingComponent.Reticle.Enable(false);
            TargetingComponent.Reticle.SetColor(TargetingComponent.NormalColor);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}