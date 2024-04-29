using System.Collections;
using UnityEngine;
using Utility;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class Fireball : SpecialAttack
    {
        const string ProjectileName = "Fireball Projectile";
        const int BaseDamage = 50;
        const float MagicPowerDamageScaling = 1.5f;
        float Damage => BaseDamage + MagicPowerDamageScaling * Stats.MagicPower;

        public override TargetingType TargetingType => new UnitTargeting();
        public override string Description => "Deals " + Damage + " damage to a single target.";
        public override int Cooldown => 3;

        public override int Range => 5;

        static readonly AssetLoader<Projectile> projectileLoader = new(Constants.VfxBundle, "Default Projectile");
        VfxObject FireballVFX => VfxLoader.LoadAsset("Blast VFX");

        protected override bool ExecuteAction(Unit origin, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;

            ActionInProgress = true;
            GameManager.ExecuteCoroutine(FireballCoroutine(origin.GridPosition, target));
            return true;
        }

        IEnumerator FireballCoroutine(Vector2Int origin, Vector2Int target)
        {
            var worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(origin);
            var projectile = projectileLoader.LoadAsset(ProjectileName);
            var projectileInstance = Object.Instantiate(projectile, worldPosition, Quaternion.identity);

            var projectileHit = false;
            projectileInstance.OnProjectileHit += () => projectileHit = true;
            projectileInstance.Fire(GameManager.CurrentLevel.Map.GetWorldPosition(target), 5);

            yield return new WaitUntil(() => projectileHit);

            var vfx = Object.Instantiate(FireballVFX, (Vector2)target, Quaternion.identity);
            vfx.Play();
            vfx.OnFinished += () => Object.Destroy(vfx.gameObject);

            var unit = GameManager.CurrentLevel.GetUnitAt(target);
            if (unit != null)
            {
                vfx.OnFinished += () => unit.Damage(Damage);
            }

            yield return new WaitForSeconds(.5f);

            ActionInProgress = false;
            OnTurnOver?.Invoke();
        }
    }
}