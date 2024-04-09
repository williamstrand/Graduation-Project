using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class ArcaneExplosion : SpecialAttack
    {
        const int Radius = 3;
        const int Damage = 10;

        static VfxObject VFX => VfxLoader.LoadAsset("Arcane Blast VFX");

        public override TargetingType TargetingType { get; } = new AuraTargeting(Radius);

        public override bool StartAction(IUnit origin, Vector2Int target)
        {
            ActionStarted = true;
            GameManager.ExecuteCoroutine(ArcaneExplosionCouroutine(origin.GridPosition));
            return true;
        }

        IEnumerator ArcaneExplosionCouroutine(Vector2Int target)
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