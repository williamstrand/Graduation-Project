﻿using System;
using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Targeting.TargetingTypes;

namespace WSP.Units.Components
{
    public class RangedAttackComponent : MonoBehaviour, IAttackComponent
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; private set; }
        public TargetingType TargetingType => new UnitTargeting();
        public Action<IUnit, bool> OnAttackHit { get; set; }

        [SerializeField] Projectile projectilePrefab;
        [SerializeField] float attackSpeed = 1;
        [SerializeField] float projectileSpeed = 5;

        public bool StartAction(IUnit attacker, Vector2Int target)
        {
            var targetUnit = GameManager.CurrentLevel.GetUnitAt(target);
            if (targetUnit == null) return false;
            if (targetUnit.GameObject == null) return false;

            var inRange = Pathfinder.Distance(attacker.GridPosition, targetUnit.GridPosition) <= attacker.Stats.AttackRange;
            if (!inRange) return false;

            ActionStarted = true;
            StartCoroutine(AttackCoroutine(attacker, targetUnit));
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