using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class Fireball : SpecialAttack
    {
        const int Damage = 50;

        public override TargetingType TargetingType => new UnitTargeting();

        VfxObject FireballVFX => VfxLoader.LoadAsset("Blast VFX");

        public override bool StartAction(IUnit origin, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;

            ActionStarted = true;
            GameManager.ExecuteCoroutine(FireballCoroutine(target));
            return true;
        }

        IEnumerator FireballCoroutine(Vector2Int target)
        {
            yield return new WaitForSeconds(.5f);

            var vfx = Object.Instantiate(FireballVFX, (Vector2)target, Quaternion.identity);
            vfx.Play();
            vfx.OnFinished += () => Object.Destroy(vfx.gameObject);

            var unit = GameManager.CurrentLevel.GetUnitAt(target);
            if (unit != null)
            {
                vfx.OnFinished += () => unit.Damage(Damage);
            }

            yield return new WaitForSeconds(.5f);

            ActionStarted = false;
            OnActionFinished?.Invoke();
        }
    }
}