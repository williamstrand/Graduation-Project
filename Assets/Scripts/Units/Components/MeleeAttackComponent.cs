using System;
using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.Components
{
    public class MeleeAttackComponent : MonoBehaviour, IAttackComponent
    {
        public Action OnTurnOver { get; set; }
        public bool ActionInProgress { get; private set; }
        public TargetingType TargetingType => new UnitTargeting();
        public string Name => "Melee Attack";
        public string Description => "Deals damage to a single target.";
        public int Cooldown => 0;
        public int CooldownRemaining { get; set; }
        public Sprite Icon => null;

        public Stats Stats { get; set; }

        public int Range { get; private set; }
        public Action<IUnit, bool> OnAttackHit { get; set; }

        [SerializeField] Transform sprite;
        [SerializeField] float attackSpeed = 1;

        public bool StartAction(IUnit attacker, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;
            if (attacker.Stats.AttackRange < Pathfinder.Distance(attacker.GridPosition, targetUnit.GridPosition)) return false;

            ActionInProgress = true;
            StartCoroutine(AttackCoroutine(attacker, targetUnit));
            return true;
        }

        public bool IsInRange(Vector2Int origin, Vector2Int target)
        {
            return Range <= 0 || Pathfinder.Distance(origin, target) <= Range;
        }

        public void SetRange(int range)
        {
            Range = range;
        }

        IEnumerator AttackCoroutine(IUnit attacker, IUnit target)
        {
            float timer = 0;
            var originalPosition = sprite.position;
            var targetPosition = GameManager.CurrentLevel.Map.GetWorldPosition(target.GridPosition);

            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                sprite.position = Vector3.Lerp(originalPosition, targetPosition, timer / 2);
                yield return null;
            }

            var targetKilled = target.Damage(Mathf.RoundToInt(attacker.Stats.Attack));
            OnAttackHit?.Invoke(target, targetKilled);

            timer = 0;
            targetPosition = originalPosition;
            originalPosition = sprite.position;
            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                sprite.position = Vector3.Lerp(originalPosition, targetPosition, timer);
                yield return null;
            }

            OnTurnOver?.Invoke();
            ActionInProgress = false;
        }
    }
}