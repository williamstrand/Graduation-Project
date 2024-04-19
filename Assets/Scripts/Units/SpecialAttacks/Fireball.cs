﻿using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class Fireball : SpecialAttack
    {
        const int BaseDamage = 50;
        const float MagicPowerDamageScaling = 1.5f;
        float Damage => BaseDamage + MagicPowerDamageScaling * Stats.MagicPower;

        public override TargetingType TargetingType => new UnitTargeting();
        public override string Description => "Deals " + Damage + " damage to a single target.";
        public override int Cooldown { get; } = 3;

        public override int Range => 5;

        VfxObject FireballVFX => VfxLoader.LoadAsset("Blast VFX");

        protected override bool ExecuteAction(IUnit origin, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;

            ActionInProgress = true;
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

            ActionInProgress = false;
            OnTurnOver?.Invoke();
        }
    }
}