using System;
using UnityEngine;

namespace WSP.Targeting
{
    public class TargetingManager : MonoBehaviour
    {
        static TargetingManager instance;

        public static Color NormalColor => instance.normalColor;
        public static Color FriendlyColor => instance.friendlyColor;
        public static Color EnemyColor => instance.enemyColor;

        [SerializeField] TargetingReticle reticle;
        [SerializeField] LineRenderer lineRenderer;

        [Header("Colors")]
        [SerializeField] Color normalColor = Color.grey;
        [SerializeField] Color friendlyColor = Color.green;
        [SerializeField] Color enemyColor = Color.red;

        Vector2Int currentOrigin;
        Vector2Int currentPosition;
        TargetingReticle.TargetType currentType;

        void Awake()
        {
            instance = this;
        }

        public static void SetTargetPosition(Vector2Int origin, Vector2Int position, TargetingReticle.TargetType type = TargetingReticle.TargetType.None)
        {
            if (instance.currentOrigin == origin && instance.currentPosition == position && instance.currentType == type) return;

            instance.currentOrigin = origin;
            instance.currentPosition = position;
            instance.currentType = type;

            if (type == TargetingReticle.TargetType.None)
            {
                instance.reticle.SetPosition(position, type);
                instance.lineRenderer.positionCount = 0;
                return;
            }

            if (GameManager.CurrentLevel.FindPath(origin, position, out var path))
            {
                instance.lineRenderer.positionCount = path.Count;
                var offset = new Vector2(GameManager.CurrentLevel.Map.CellSize / 2, GameManager.CurrentLevel.Map.CellSize / 2);

                for (var i = 0; i < path.Count; i++)
                {
                    instance.lineRenderer.SetPosition(i, path[i].Position + offset);
                }

                var color = type switch
                {
                    TargetingReticle.TargetType.Normal => instance.normalColor,
                    TargetingReticle.TargetType.Friendly => instance.friendlyColor,
                    TargetingReticle.TargetType.Enemy => instance.enemyColor,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                instance.lineRenderer.startColor = color;
                instance.lineRenderer.endColor = color;
            }
            else
            {
                instance.lineRenderer.positionCount = 0;
            }

            instance.reticle.SetPosition(position, type);
            instance.reticle.Enable(true);
        }
    }
}