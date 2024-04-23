using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using WSP.Map.Pathfinding;

namespace WSP.Map
{
    public class FogOfWar : MonoBehaviour
    {
        readonly Color fullFog = Color.black.Alpha(1);
        readonly Color halfFog = Color.black.Alpha(0.5f);

        [SerializeField] GameObject fogTilePrefab;

        Dictionary<Vector2Int, GameObject> fog = new();
        HashSet<Vector2Int> found = new();

        Queue<GameObject> pool = new();

        public void Set(Vector2Int position, bool visible)
        {
            GameObject fogTile;
            Vector2 worldPosition;

            if (visible)
            {
                found.Add(position);
                if (fog.TryGetValue(position, out fogTile))
                {
                    Remove(fogTile);
                    fog.Remove(position);
                }

                var neighbours = GameManager.CurrentLevel.Map.GetNeighbours(position, found);
                for (var i = 0; i < neighbours.Length; i++)
                {
                    if (fog.TryGetValue(neighbours[i], out fogTile))
                    {
                        Remove(fogTile);
                        fog.Remove(neighbours[i]);
                    }

                    worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(neighbours[i]);
                    fogTile = GetFromPool();
                    fogTile.transform.position = worldPosition;
                    fogTile.GetComponent<SpriteRenderer>().color = halfFog;
                    fogTile.hideFlags = HideFlags.HideInHierarchy;
                    fog.Add(neighbours[i], fogTile);
                    found.Add(neighbours[i]);
                }

                return;
            }

            if (fog.ContainsKey(position)) return;

            worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(position);

            fogTile = GetFromPool();
            fogTile.transform.position = worldPosition;
            fogTile.GetComponent<SpriteRenderer>().color = found.Contains(position) ? halfFog : fullFog;
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
                Set(fill[i], visible);
            }
        }

        GameObject GetFromPool()
        {
            if (pool.Count <= 0) return Instantiate(fogTilePrefab, transform);

            var fogTile = pool.Dequeue();
            fogTile.SetActive(true);
            return fogTile;
        }

        void Remove(GameObject fogTile)
        {
            fogTile.SetActive(false);
            pool.Enqueue(fogTile);
        }
    }
}