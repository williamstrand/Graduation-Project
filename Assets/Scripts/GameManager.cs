using UnityEngine;
using WSP.Map;
using Grid = WSP.Map.Grid;

namespace WSP
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject square;
        [SerializeField] GameObject exit;
        Transform parent;
        MapGenerator mapGenerator = new();

        void Start()
        {
            parent = new GameObject("Map").transform;
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

            mapGenerator.StartRoomWidth = new Vector2Int(10, 10);
            mapGenerator.StartRoomHeight = new Vector2Int(10, 10);
            mapGenerator.RoomWidth = new Vector2Int(5, 10);
            mapGenerator.RoomHeight = new Vector2Int(5, 10);
            var grid = mapGenerator.GenerateMap(40, 40, 1f, 10);
            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                    switch (grid.GetValue(x, y))
                    {
                        case Grid.Empty:
                            break;

                        case Grid.Wall:
                            Instantiate(square, grid.GetWorldPosition(x, y), Quaternion.identity, parent);
                            break;

                        case Grid.Exit:
                            Instantiate(exit, grid.GetWorldPosition(x, y), Quaternion.identity, parent);
                            break;
                    }
            }
        }
    }
}