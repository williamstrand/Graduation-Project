using System;
using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class DefaultTargeting : TargetingType
    {
        public override void StartTarget(TargetingManager targetingManager)
        {
            base.StartTarget(targetingManager);

            TargetingManager.ShouldDrawPath = true;
            TargetingManager.Reticle.Enable(true);
        }

        public override void Target(Vector2Int origin, Vector2Int target)
        {
            if (TargetingManager.Reticle.Type == TargetingReticle.ReticleTargetType.None)
            {
                TargetingManager.Reticle.Enable(false);
                TargetingManager.ShouldDrawPath = false;
                return;
            }

            TargetingManager.Reticle.Enable(true);
            TargetingManager.ShouldDrawPath = true;
            TargetingManager.Reticle.SetPosition(target);
            TargetingManager.Reticle.SetColor(TargetingManager.Reticle.Type switch
            {
                TargetingReticle.ReticleTargetType.Normal => TargetingManager.NormalColor,
                TargetingReticle.ReticleTargetType.Friendly => TargetingManager.FriendlyColor,
                TargetingReticle.ReticleTargetType.Enemy => TargetingManager.EnemyColor,
                TargetingReticle.ReticleTargetType.None => Color.clear,
                _ => throw new ArgumentOutOfRangeException(nameof(TargetingManager.Reticle.Type), TargetingManager.Reticle.Type, null)
            });
        }

        public override void StopTarget()
        {
            TargetingManager.ShouldDrawPath = false;
            TargetingManager.Reticle.Enable(false);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}