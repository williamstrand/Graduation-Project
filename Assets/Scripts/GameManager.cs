using UnityEngine;
using WSP.Map;
using WSP.Map.Pathfinding;

namespace WSP
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject square;
        [SerializeField] GameObject exit;
        Transform parent;
        MapGenerator mapGenerator;

        void Start()
        {
            parent = new GameObject("Map").transform;
            mapGenerator = new MapGenerator
            {
                StartRoomWidth = new Vector2Int(10, 10),
                StartRoomHeight = new Vector2Int(10, 10),
                RoomWidth = new Vector2Int(5, 10),
                RoomHeight = new Vector2Int(5, 10),
                MaxRooms = 10,
                Width = 40,
                Height = 40,
                CellSize = 1f
            };
            GenerateMap();
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                GenerateMap();
            }
        }

        void GenerateMap()
        {
            foreach (Transform tile in parent) Destroy(tile.gameObject);

            var grid = mapGenerator.GenerateMap();
            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                    switch (grid.GetValue(x, y))
                    {
                        case Map.Map.Empty:
                            break;

                        case Map.Map.Wall:
                            Instantiate(square, grid.GetWorldPosition(x, y), Quaternion.identity, parent);
                            break;

                        case Map.Map.Exit:
                            Instantiate(exit, grid.GetWorldPosition(x, y), Quaternion.identity, parent);
                            break;
                    }
            }

            var startRoom = grid.StartRoom;
            var exitRoom = grid.ExitRoom;

            if (Pathfinder.FindPath(grid, startRoom.Center, exitRoom.Center, out var path))
            {
                for (var i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(grid.GetWorldPosition(path[i - 1].Position), grid.GetWorldPosition(path[i].Position), Color.green, 5f);
                }
            }
        }
    }
}