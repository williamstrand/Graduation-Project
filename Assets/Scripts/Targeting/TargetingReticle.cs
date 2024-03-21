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

        public ReticleTargetType Type { get; set; }

        public void SetSize(Vector2 size)
        {
            spriteRenderer.size = size;
        }

        public void SetPosition(Vector2Int position)
        {
            switch (Type)
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
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
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