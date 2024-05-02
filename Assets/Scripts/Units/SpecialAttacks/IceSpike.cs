﻿using System.Collections;
using UnityEngine;
using WSP.Targeting.TargetingTypes;
using WSP.VFX;

namespace WSP.Units.SpecialAttacks
{
    public class IceSpike : SpecialAttack
    {
        public override TargetingType TargetingType { get; } = new LineTargeting(10);
        public override string Description => "Deals " + Damage + " damage to the first enemy in a line.";
        public override int Cooldown { get; } = 5;

        const int BaseDamage = 15;
        const float MagicScaling = 2f;
        float Damage => BaseDamage + MagicScaling * Stats.MagicPower;

        VfxObject iceSpikePrefab = VfxLoader.LoadAsset("Ice Spike VFX");

        protected override bool ExecuteAction(Unit origin, Vector2Int target)
        {
            ActionInProgress = true;
            GameManager.ExecuteCoroutine(IceSpikeCoroutine(origin.GridPosition, target));
            return true;
        }

        IEnumerator IceSpikeCoroutine(Vector2Int origin, Vector2Int target)
        {
            var targets = TargetingType.GetTargets(origin, target);
            var length = targets.Length;
            var diff = target - origin;
            var direction = new Vector2Int(diff.x == 0 ? 0 : diff.x / Mathf.Abs(diff.x), diff.y == 0 ? 0 : diff.y / Mathf.Abs(diff.y));
            var iceSpike = Object.Instantiate(iceSpikePrefab, (Vector2)origin, Quaternion.identity);
            iceSpike.transform.right = (Vector2)direction;


            for (var i = 0; i < length - 1; i++)
            {
                var t = 0f;
                var currentPosition = targets[i];
                var nextPosition = targets[i + 1];

                while (t < 1)
                {
                    iceSpike.transform.position = Vector2.Lerp(GameManager.CurrentLevel.Map.GetWorldPosition(currentPosition),
                        GameManager.CurrentLevel.Map.GetWorldPosition(nextPosition), t);
                    t += Time.deltaTime * 5;
                    yield return null;
                }

                var unit = GameManager.CurrentLevel.GetUnitAt(nextPosition);

                if (unit == null) continue;

                unit.Damage(Damage);
                break;
            }

            Object.Destroy(iceSpike.gameObject);
            ActionInProgress = false;
            OnTurnOver?.Invoke();
        }
    }
}