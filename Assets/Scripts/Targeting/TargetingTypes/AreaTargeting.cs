using UnityEngine;

namespace WSP.Targeting.TargetingTypes
{
    public class AreaTargeting : TargetingType
    {
        int width;
        int height;
        TargetingReticle currentReticle;

        public AreaTargeting(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Target(Vector2Int origin, Vector2Int target, TargetingReticle reticle)
        {
            currentReticle ??= reticle;

            reticle.SetSize(new Vector2(width * GameManager.CurrentLevel.Map.CellSize, height * GameManager.CurrentLevel.Map.CellSize));
            reticle.SetPosition(target);
            reticle.Enable(true);
        }

        public override void StopTarget()
        {
            currentReticle.SetSize(new Vector2(1, 1));
        }
    }
}