using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class MeleeAttackComponent : AttackComponent
    {
        public override string Name => "Melee Attack";
        public override string Description => "Deals damage to a single target.";

        [SerializeField] Transform sprite;
        [SerializeField] float attackSpeed = 1;

        public override bool StartAction(IUnit attacker, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;
            if (attacker.Stats.AttackRange < Pathfinder.Distance(attacker.GridPosition, targetUnit.GridPosition)) return false;

            ActionInProgress = true;
            StartCoroutine(AttackCoroutine(attacker, targetUnit));
            return true;
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