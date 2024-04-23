using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using WSP.Map.Pathfinding;

namespace WSP.Map
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] GameObject fogTilePrefab;

        Dictionary<Vector2Int, GameObject> fog = new();
        HashSet<Vector2Int> found = new();

        public void Set(Vector2Int position, bool visible)
        {
            if (GameManager.CurrentLevel.Map.GetValue(position) == Pathfinding.Map.Wall) return;

            GameObject fogTile;

            if (visible)
            {
                if (!fog.TryGetValue(position, out fogTile)) return;

                found.Add(position);

                Destroy(fogTile);
                fog.Remove(position);

                return;
            }

            if (fog.ContainsKey(position)) return;

            var worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(position);

            fogTile = Instantiate(fogTilePrefab, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity, transform);
            fogTile.GetComponent<SpriteRenderer>().color = Color.black.Alpha(found.Contains(position) ? 0.5f : 1);
            fogTile.hideFlags = HideFlags.HideInHierarchy;
            fog.Add(position, fogTile);
        }

        public void SetArea(Pathfinding.Map map, Vector2Int position, int radius, bool visible)
        {
            var keys = found.ToList();

            for (var i = 0; i < keys.Count; i++)
            {
                Set(keys[i], false);
            }

            var fill = Pathfinder.FloodFill(map, position, radius);

            for (var i = 0; i < fill.Length; i++)
            {
                if (GameManager.CurrentLevel.Map.GetValue(fill[i]) == Pathfinding.Map.Wall) continue;

                Set(fill[i], visible);
            }
        }
    }
}