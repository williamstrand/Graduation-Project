using System;
using System.Collections;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Units.Components
{
    public class AttackComponent : MonoBehaviour, IAttackComponent
    {
        public Action OnActionFinished { get; set; }
        public bool ActionStarted { get; private set; }
        public TargetingType TargetingType => TargetingType.Unit;
        public Action<IUnit, bool> OnAttackHit { get; set; }

        [SerializeField] Transform sprite;
        [SerializeField] float attackSpeed = 1;

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
            float timer = 0;
            var originalPosition = sprite.position;
            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                sprite.position = Vector3.Lerp(originalPosition, target.GameObject.transform.position, timer / 2);
                yield return null;
            }

            var targetKilled = target.Damage(Mathf.RoundToInt(attacker.Stats.Attack));
            OnAttackHit?.Invoke(target, targetKilled);

            timer = 0;
            originalPosition = sprite.position;
            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                sprite.position = Vector3.Lerp(originalPosition, transform.position, timer);
                yield return null;
            }

            OnActionFinished?.Invoke();
            ActionStarted = false;
        }
    }
}