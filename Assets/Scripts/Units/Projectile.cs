using System;
using UnityEngine;

namespace WSP.Units
{
    public class Projectile : MonoBehaviour
    {
        public Action OnProjectileHit { get; set; }

        Vector2 targetPosition;
        float speed;
        bool isFired;

        void Awake()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        public void Fire(Vector2 target, float projectileSpeed)
        {
            targetPosition = target;
            speed = projectileSpeed;
            isFired = true;
            var directionToTarget = target - (Vector2)transform.position;
            transform.right = directionToTarget.normalized;
        }

        void Update()
        {
            if (!isFired) return;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if ((Vector2)transform.position != targetPosition) return;

            OnProjectileHit?.Invoke();
            isFired = false;
            Destroy();
        }

        void Destroy()
        {
            OnProjectileHit = null;
            Destroy(gameObject);
        }
    }
}