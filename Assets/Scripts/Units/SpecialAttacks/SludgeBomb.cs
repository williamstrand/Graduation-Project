using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units.Buffs;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class SludgeBomb : SpecialAttack
    {
        const int BaseDamage = 10;
        const float MagicPowerDamageScaling = 1.1f;
        float Damage => BaseDamage + MagicPowerDamageScaling * Stats.MagicPower;

        const int PoisonDamage = 5;
        const int PoisonDuration = 10;

        public override string Name => "Sludge Bomb";
        public override TargetingType TargetingType { get; } = new AreaTargeting(3, 3);
        public override string Description => $"Deals {Damage} damage to all units in an area and poisons all units hit for {PoisonDuration} turns.";
        public override int Cooldown { get; } = 6;
        public override int Range => 3;

        static VfxObject VFX => VfxLoader.LoadAsset("Poison Blast VFX");

        protected override bool ExecuteAction(Unit origin, Vector2Int target)
        {
            ActionInProgress = true;
            origin.StartCoroutine(SludgeBombCoroutine(origin.GridPosition, target));
            return true;
        }

        IEnumerator SludgeBombCoroutine(Vector2Int origin, Vector2Int target)
        {
            yield return new WaitForSeconds(.5f);

            var targets = TargetingType.GetTargets(Vector2Int.zero, target);
            for (var i = 0; i < targets.Length; i++)
            {
                var vfx = Object.Instantiate(VFX, (Vector2)targets[i], Quaternion.identity);
                vfx.Play();
                vfx.OnFinished += () => Object.Destroy(vfx.gameObject);

                var unit = GameManager.CurrentLevel.GetUnitAt(targets[i]);
                if (unit != null)
                {
                    vfx.OnFinished += () => DamageUnit(unit);
                }

                yield return new WaitForSeconds(.05f);
            }

            yield return new WaitForSeconds(.25f);

            ActionInProgress = false;
            OnTurnOver?.Invoke();
        }

        void DamageUnit(Unit unit)
        {
            unit.Damage(Damage);
            var poison = new PoisonDebuff(PoisonDuration, PoisonDamage);
            poison.Apply(unit);
        }
    }
}