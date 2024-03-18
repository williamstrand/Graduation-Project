using System;
using UnityEngine;

namespace WSP.Targeting
{
    public class TargetingReticle : MonoBehaviour
    {
        public enum TargetType
        {
            None,
            Normal,
            Friendly,
            Enemy
        }

        [SerializeField] SpriteRenderer spriteRenderer;


        public void SetPosition(Vector2Int position, TargetType type = TargetType.None)
        {
            switch (type)
            {
                case TargetType.None:
                    Enable(false);
                    return;

                case TargetType.Normal:

                    spriteRenderer.color = TargetingManager.NormalColor;
                    break;

                case TargetType.Friendly:
                    spriteRenderer.color = TargetingManager.FriendlyColor;
                    break;

                case TargetType.Enemy:
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