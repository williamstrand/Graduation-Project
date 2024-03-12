using System;
using System.Collections;
using UnityEngine;

namespace WSP.Units.Components
{
    public class AttackComponent : MonoBehaviour, IAttackComponent
    {
        public Action OnAttackFinished { get; set; }

        [SerializeField] Transform sprite;
        [SerializeField] float attackSpeed = 1;

        public void Attack(IUnit attacker, IUnit target)
        {
            StartCoroutine(AttackCoroutine(attacker, target));
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

            target.Damage(attacker.Stats.Attack);

            timer = 0;
            originalPosition = sprite.position;
            while (timer < 1)
            {
                timer += Time.deltaTime * attackSpeed * 2;
                sprite.position = Vector3.Lerp(originalPosition, transform.position, timer);
                yield return null;
            }

            OnAttackFinished?.Invoke();
        }
    }
}