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
            Enable(true);
            transform.position = new Vector3(position.x, position.y, 0);
        }

        public void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}