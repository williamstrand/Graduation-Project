using System;
using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class DefaultTargeting : TargetingType
    {
        public override void StartTarget(TargetingComponent targetingComponent)
        {
            base.StartTarget(targetingComponent);

            TargetingComponent.ShowPath();
            TargetingComponent.Reticle.Enable(true);
        }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (TargetingComponent.Reticle.Type == TargetingReticle.ReticleTargetType.None)
            {
                TargetingComponent.Reticle.Enable(false);
                TargetingComponent.HidePath();
                return;
            }

            TargetingComponent.Reticle.Enable(true);
            TargetingComponent.ShowPath();
            TargetingComponent.Reticle.SetPosition(target);
            TargetingComponent.Reticle.SetColor(TargetingComponent.Reticle.Type switch
            {
                TargetingReticle.ReticleTargetType.Normal => TargetingComponent.NormalColor,
                TargetingReticle.ReticleTargetType.Friendly => TargetingComponent.FriendlyColor,
                TargetingReticle.ReticleTargetType.Enemy => TargetingComponent.EnemyColor,
                TargetingReticle.ReticleTargetType.None => Color.clear,
                _ => throw new ArgumentOutOfRangeException(nameof(TargetingComponent.Reticle.Type), TargetingComponent.Reticle.Type, null)
            });
        }

        public override void StopTarget()
        {
            TargetingComponent.HidePath();
            TargetingComponent.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}