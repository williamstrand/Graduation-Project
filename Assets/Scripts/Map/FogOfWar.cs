using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using WSP.Map.Pathfinding;

namespace WSP.Map
{
    public class FogOfWar : MonoBehaviour
    {
        public Action OnFogChange { get; set; }

        [SerializeField] Color fogColor = Color.black;

        Color FullFog => fogColor.Alpha(1);
        Color HalfFog => fogColor.Alpha(0.5f);

        [SerializeField] GameObject fogTilePrefab;

        Dictionary<Vector2Int, SpriteRenderer> fog = new();
        HashSet<Vector2Int> found = new();

        Queue<SpriteRenderer> pool = new();

        public void Set(Vector2Int position, bool visible)
        {
            SpriteRenderer fogTile;
            Vector2 worldPosition;

            if (visible)
            {
                found.Add(position);
                Remove(position);

                var neighbours = GameManager.CurrentLevel.Map.GetNeighbours(position, found);
                for (var i = 0; i < neighbours.Length; i++)
                {
                    Remove(neighbours[i]);

                    worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(neighbours[i]);
                    fogTile = GetFromPool();
                    fogTile.transform.position = worldPosition;
                    fogTile.color = HalfFog;
                    fog.Add(neighbours[i], fogTile);
                    found.Add(neighbours[i]);
                }

                OnFogChange?.Invoke();
                return;
            }

            if (fog.ContainsKey(position)) return;

            worldPosition = GameManager.CurrentLevel.Map.GetWorldPosition(position);

            fogTile = GetFromPool();
            fogTile.transform.position = worldPosition;
            fogTile.color = found.Contains(position) ? HalfFog : FullFog;
            fog.Add(position, fogTile);
            OnFogChange?.Invoke();
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

        SpriteRenderer GetFromPool()
        {
            SpriteRenderer fogTile;
            if (pool.Count <= 0)
            {
                fogTile = Instantiate(fogTilePrefab, transform).GetComponent<SpriteRenderer>();
                fogTile.gameObject.hideFlags = HideFlags.HideInHierarchy;
                return fogTile;
            }

            fogTile = pool.Dequeue();
            fogTile.gameObject.SetActive(true);
            return fogTile;
        }

        void Remove(Vector2Int position)
        {
            if (!fog.TryGetValue(position, out var fogTile)) return;

            fog.Remove(position);
            fogTile.gameObject.SetActive(false);
            pool.Enqueue(fogTile);
        }

        public bool IsHidden(Vector2Int position)
        {
            return fog.ContainsKey(position);
        }

        public bool IsFound(Vector2Int position)
        {
            return found.Contains(position);
        }
    }
}