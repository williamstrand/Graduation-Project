using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class ArcaneExplosion : SpecialAttack
    {
        const int Radius = 3;
        const int BaseDamage = 10;
        const float MagicPowerDamageScaling = 1.5f;
        float Damage => BaseDamage + MagicPowerDamageScaling * Stats.MagicPower;

        static VfxObject VFX => VfxLoader.LoadAsset("Arcane Blast VFX");

        public override TargetingType TargetingType { get; } = new AuraTargeting(Radius);
        public override string Name => "Arcane Explosion";
        public override string Description => "Deals " + Damage + " damage to all enemies in a " + Mathf.FloorToInt((float)Radius / 2) + " tile radius.";
        public override int Cooldown { get; } = 4;

        protected override bool ExecuteAction(IUnit origin, Vector2Int target)
        {
            ActionStarted = true;
            GameManager.ExecuteCoroutine(ArcaneExplosionCoroutine(origin.GridPosition));
            return true;
        }

        IEnumerator ArcaneExplosionCoroutine(Vector2Int target)
        {
            var targets = TargetingType.GetTargets(target, target);

            var vfx = Object.Instantiate(VFX, (Vector2)target, Quaternion.identity);
            vfx.Play();

            for (var i = 0; i < targets.Length; i++)
            {
                var unit = GameManager.CurrentLevel.GetUnitAt(targets[i]);
                if (unit != null)
                {
                    vfx.OnFinished += () => DamageUnit(unit);
                }
            }

            vfx.OnFinished += () => Object.Destroy(vfx.gameObject);

            yield return new WaitForSeconds(1);

            ActionStarted = false;
            OnActionFinished?.Invoke();
        }

        void DamageUnit(IUnit unit)
        {
            unit.Damage(Damage);
        }
    }
}