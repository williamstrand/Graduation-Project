using UnityEngine;

namespace WSP.Map
{
    public class Grid
    {
        public const int Empty = 0;
        public const int Wall = 1;
        public const int Exit = 2;

        int[,] gridArray;
        float cellSize;

        public int Height => gridArray.GetLength(1);
        public int Width => gridArray.GetLength(0);

        public Grid(int width, int height, float cellSize)
        {
            gridArray = new int[width, height];
            this.cellSize = cellSize;
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * cellSize;
        }

        public Vector2 GetWorldPosition(Vector2Int gridPosition)
        {
            return GetWorldPosition(gridPosition.x, gridPosition.y);
        }

        public Vector2Int GetGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x / cellSize), Mathf.FloorToInt(worldPosition.y / cellSize));
        }

        public void SetValue(int x, int y, int value)
        {
            if (value is < Empty or > Exit)
            {
                Debug.LogError("Value out of range");
                return;
            }

            if (x >= 0 && y >= 0 && x < gridArray.GetLength(0) && y < gridArray.GetLength(1))
            {
                gridArray[x, y] = value;
            }
        }

        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < gridArray.GetLength(0) && y < gridArray.GetLength(1))
            {
                return gridArray[x, y];
            }

            return -1;
        }
    }
}