using System;
using UnityEngine;

namespace WSP.Targeting
{
    public class TargetingReticle : MonoBehaviour
    {
        public enum ReticleTargetType
        {
            None,
            Normal,
            Friendly,
            Enemy
        }

        [SerializeField] SpriteRenderer spriteRenderer;


        public void SetPosition(Vector2Int position, ReticleTargetType type = ReticleTargetType.None)
        {
            switch (type)
            {
                case ReticleTargetType.None:
                    Enable(false);
                    return;

                case ReticleTargetType.Normal:

                    spriteRenderer.color = TargetingManager.NormalColor;
                    break;

                case ReticleTargetType.Friendly:
                    spriteRenderer.color = TargetingManager.FriendlyColor;
                    break;

                case ReticleTargetType.Enemy:
                    spriteRenderer.color = TargetingManager.EnemyColor;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            Enable(true);
            transform.position = new Vector3(position.x, position.y, 0);
        }

        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}