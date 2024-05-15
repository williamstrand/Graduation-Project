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

        [field: Header("Colors")]
        [field: SerializeField] public Color NormalColor { get; private set; } = Color.white;
        [field: SerializeField] public Color FriendlyColor { get; private set; } = Color.green;
        [field: SerializeField] public Color EnemyColor { get; private set; } = Color.red;

        public ReticleTargetType Type { get; private set; }
        public Color Color => spriteRenderer.color;

        public void SetSize(Vector2 size)
        {
            spriteRenderer.size = size;
        }

        public void SetPosition(Vector2Int position)
        {
            Enable(true);
            transform.position = new Vector3(position.x, position.y, 0);
        }

        public void SetType(ReticleTargetType type)
        {
            Type = type;
            spriteRenderer.color = type switch
            {
                ReticleTargetType.Normal => NormalColor,
                ReticleTargetType.Friendly => FriendlyColor,
                ReticleTargetType.Enemy => EnemyColor,
                ReticleTargetType.None => Color.clear,
                _ => Color.clear
            };
        }

        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void Reset()
        {
            SetType(ReticleTargetType.Normal);
            SetSize(Vector2.one);
        }
    }
}