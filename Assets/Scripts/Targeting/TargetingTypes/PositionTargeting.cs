using UnityEngine;
using WSP.Units;

namespace WSP.Targeting.TargetingTypes
{
    public class PositionTargeting : TargetingType
    {
        protected TargetingReticle Reticle;

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

            Reticle = GetReticle();
            Reticle.SetPosition(target);
            switch (GameManager.CurrentLevel.GetObjectAt(target))
            {
                case null:
                    Reticle.SetType(TargetingReticle.ReticleTargetType.Normal);
                    break;

                case Unit unit:
                    if (ReferenceEquals(unit, GameManager.CurrentLevel.Player.Unit))
                    {
                        Reticle.SetType(TargetingReticle.ReticleTargetType.None);
                    }
                    else
                    {
                        Reticle.SetType(TargetingReticle.ReticleTargetType.Enemy);
                    }

                    break;
            }

            TargetingComponent.ShowPath(Reticle.Color);
        }

        public override Vector2Int[] GetTargets(Vector2Int origin, Vector2Int target)
        {
            return new[] { target };
        }
    }
}