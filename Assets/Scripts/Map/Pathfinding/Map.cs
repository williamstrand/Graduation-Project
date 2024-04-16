using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map.Pathfinding
{
    public class Map
    {
        public const int Invalid = -1;
        public const int Empty = 0;
        public const int Wall = 1;

        Dictionary<Vector2Int, int> gridArray;

        public float CellSize;
        public int Height { get; }
        public int Width { get; }
        public List<Room> Rooms { get; set; } = new();
        public Room ExitRoom { get; set; }
        public Room StartRoom { get; set; }

        public Map(int width, int height, float cellSize)
        {
            Width = width;
            Height = height;
            gridArray = new Dictionary<Vector2Int, int>();
            CellSize = cellSize;
        }

        /// <summary>
        ///     Converts a grid position to a world position.
        /// </summary>
        /// <param name="x">the grid x position.</param>
        /// <param name="y">the grid y position.</param>
        /// <returns>the world position.</returns>
        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * CellSize + new Vector2(CellSize / 2, CellSize / 2);
        }

        /// <summary>
        ///     Converts a grid position to a world position.
        /// </summary>
        /// <param name="gridPosition">the grid position.</param>
        /// <returns>the world position.</returns>
        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return GetWorldPosition(gridPosition.x, gridPosition.y);
        }

        /// <summary>
        ///     Converts a world position to a grid position.
        /// </summary>
        /// <param name="worldPosition">the world position.</param>
        /// <returns>a grid position.</returns>
        public Vector2Int GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x / CellSize), Mathf.FloorToInt(worldPosition.y / CellSize));
        }

        /// <summary>
        ///     Sets the value of a grid position.
        /// </summary>
        /// <param name="x">the grid x position.</param>
        /// <param name="y">the grid y position.</param>
        /// <param name="value">the value to set the grid position to.</param>
        public void SetValue(int x, int y, int value)
        {
            if (value is < Empty or > Wall)
            {
                Debug.LogError("Value out of range");
                return;
            }

            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                gridArray[new Vector2Int(x, y)] = value;
            }
        }

        public void SetValue(Vector2Int gridPosition, int value)
        {
            SetValue(gridPosition.x, gridPosition.y, value);
        }

        /// <summary>
        ///     Gets the value of a grid position.
        /// </summary>
        /// <param name="gridPosition">the grid position.</param>
        /// <returns>the value.</returns>
        public int GetValue(Vector2Int gridPosition)
        {
            return GetValue(gridPosition.x, gridPosition.y);
        }

        /// <summary>
        ///     Gets the value of a grid position.
        /// </summary>
        /// <param name="x">the grid x position.</param>
        /// <param name="y">the grid y position.</param>
        /// <returns>the value.</returns>
        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                return gridArray[new Vector2Int(x, y)];
            }

            return Invalid;
        }

        /// <summary>
        ///     Gets a list of the neighbours of a grid position.
        /// </summary>
        /// <param name="gridPosition">the grid position.</param>
        /// <param name="ignore">list of positions to ignore.</param>
        /// <returns>a list of neighbours.</returns>
        public Vector2Int[] GetNeighbours(Vector2Int gridPosition, ICollection<Vector2Int> ignore)
        {
            var neighbours = new Vector2Int[8];
            var i = 0;
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    var checkX = gridPosition.x + x;
                    var checkY = gridPosition.y + y;
                    var vector = new Vector2Int(checkX, checkY);
                    if (ignore.Contains(vector)) continue;

                    if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
                    {
                        neighbours[i] = vector;
                        i++;
                    }
                }
            }

            return neighbours;
        }

        public Map Copy()
        {
            var map = new Map(Width, Height, CellSize)
            {
                gridArray = new Dictionary<Vector2Int, int>(gridArray),
                Rooms = new List<Room>(Rooms),
                StartRoom = StartRoom,
                ExitRoom = ExitRoom
            };

            return map;
        }

        public Room GetRandomRoom()
        {
            return Rooms[Random.Range(0, Rooms.Count)];
        }
    }
}