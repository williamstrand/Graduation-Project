using System;
using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class RangedAttackComponent : MonoBehaviour, IAttackComponent
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; private set; }
        public TargetingType TargetingType => TargetingType.Unit;
        public Action<IUnit, bool> OnAttackHit { get; set; }

        [SerializeField] Projectile projectilePrefab;
        [SerializeField] float attackSpeed = 1;
        [SerializeField] float projectileSpeed = 5;

        public bool StartAction(IUnit attacker, ActionTarget target)
        {
            if (target.TargetUnit == null) return false;
            if (attacker.Stats.AttackRange < Pathfinder.Distance(attacker.GridPosition, target.TargetUnit.GridPosition)) return false;

            ActionStarted = true;
            StartCoroutine(AttackCoroutine(attacker, target.TargetUnit));
            return true;
        }

        IEnumerator AttackCoroutine(IUnit attacker, IUnit target)
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

        void OnProjectileHit(IUnit attacker, IUnit target)
        {
            var targetKilled = target.Damage(Mathf.RoundToInt(attacker.Stats.Attack));
            OnAttackHit?.Invoke(target, targetKilled);

            OnActionFinished?.Invoke();
            ActionStarted = false;
        }
    }
}