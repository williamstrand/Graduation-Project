using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class RangedAttackComponent : AttackComponent
    {
        public override string Name => "Ranged Attack";
        public override string Description => "Deals damage to a single target.";

        [SerializeField] Projectile projectilePrefab;
        [SerializeField] float attackSpeed = 1;
        [SerializeField] float projectileSpeed = 5;

        public override bool StartAction(Unit attacker, Vector2Int target, bool visible)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;
            if (targetUnit.GameObject == null) return false;

            var inRange = Pathfinder.Distance(attacker.GridPosition, targetUnit.GridPosition) <= attacker.Stats.AttackRange;
            if (!inRange) return false;

            ActionInProgress = true;
            StartCoroutine(AttackCoroutine(attacker, targetUnit));
            return true;
        }

        IEnumerator AttackCoroutine(Unit attacker, Unit target)
        {
            var timer = 0f;
            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                yield return null;
            }

            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.OnProjectileHit += () => { OnProjectileHit(attacker, target); };
            projectile.Fire(GameManager.CurrentLevel.Map.GetWorldPosition(target.GridPosition), projectileSpeed);
        }

        void OnProjectileHit(Unit attacker, Unit target)
        {
            var targetKilled = target.Damage(Mathf.RoundToInt(attacker.Stats.Attack));

            OnTurnOver?.Invoke();
            ActionInProgress = false;
        }
    }
}