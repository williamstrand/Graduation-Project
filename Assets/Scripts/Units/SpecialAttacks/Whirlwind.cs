using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class Whirlwind : SpecialAttack
    {
        const int Radius = 3;
        const int BaseDamage = 3;
        const float AttackDamageScaling = .5f;
        float Damage => BaseDamage + AttackDamageScaling * Stats.Attack;
        const int Spins = 3;

        public override TargetingType TargetingType { get; } = new AuraTargeting(Radius);
        public override string Description => "Deals " + Damage + " damage to all enemies in a " + Mathf.FloorToInt((float)Radius / 2) + " tile radius, " + Spins + "times.";
        public override int Cooldown { get; } = 3;

        static VfxObject VFX => VfxLoader.LoadAsset("Whirlwind VFX");

        protected override bool ExecuteAction(Unit origin, Vector2Int target)
        {
            ActionInProgress = true;
            origin.StartCoroutine(WhirlwindCoroutine(origin.GridPosition));
            return true;
        }

        IEnumerator WhirlwindCoroutine(Vector2Int target)
        {
            var targets = TargetingType.GetTargets(target, target);

            for (var i = 0; i < Spins; i++)
            {
                var vfx = Object.Instantiate(VFX, (Vector2)target, Quaternion.identity);
                vfx.Play();

                for (var j = 0; j < targets.Length; j++)
                {
                    var unit = GameManager.CurrentLevel.GetUnitAt(targets[j]);
                    if (unit != null)
                    {
                        vfx.OnFinished += () => DamageUnit(unit);
                    }
                }

                if (i == Spins - 1) vfx.OnFinished += EndAction;
                vfx.OnFinished += () => Object.Destroy(vfx.gameObject);

                yield return new WaitForSeconds(.25f);
            }

            yield break;

            void EndAction()
            {
                ActionInProgress = false;
                OnTurnOver?.Invoke();
            }
        }

        void DamageUnit(Unit unit)
        {
            unit.Damage(Damage);
        }
    }
}