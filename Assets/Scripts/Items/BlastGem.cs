﻿using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.Units;
using WSP.VFX;

namespace WSP.Items
{
    public class BlastGem : Item
    {
        const int Width = 3;
        const int Height = 3;
        const int Damage = 50;

        public override string Name => "Blast Gem";
        public override string Description => "Triggers an explosion.";
        public override int Weight => 30;
        public override TargetingType TargetingType { get; } = new AreaTargeting(Width, Height);

        static VfxObject VFX => VfxLoader.LoadAsset("Blast VFX");

        protected override bool ActivateEffect(IUnit origin, Vector2Int target)
        {
            ActionStarted = true;
            GameManager.ExecuteCoroutine(BlastCouroutine(target));
            return true;
        }

        IEnumerator BlastCouroutine(Vector2Int target)
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