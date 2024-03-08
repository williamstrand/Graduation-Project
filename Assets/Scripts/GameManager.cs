using UnityEngine;
using WSP.Map;
using WSP.Map.Pathfinding;
using WSP.Units;
using WSP.Units.Player;

namespace WSP
{
    public class GameManager : MonoBehaviour
    {
        public static Map.Map CurrentMap { get; private set; }
        public static bool IsPlayerTurn { get; private set; } = true;
        [SerializeField] GameObject square;
        [SerializeField] GameObject exit;
        Transform mapParent;
        [SerializeField] PlayerController playerPrefab;
        MapGenerator mapGenerator;
        IUnit player;
        float timer;

        void Awake()
        {
            Singleton<GameManager>.Initialize(this);
        }

        void Start()
        {
            mapParent = new GameObject("Map").transform;
            GenerateMap();

            player = Instantiate(playerPrefab, CurrentMap.GetWorldPosition(CurrentMap.StartRoom.Center), Quaternion.identity);
            player.OnTurnEnd += EndPlayerTurn;
        }

        void Update()
        {
            if (IsPlayerTurn) return;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                IsPlayerTurn = true;
            }
        }

        void GenerateMap()
        {
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

            foreach (Transform tile in mapParent) Destroy(tile.gameObject);

            CurrentMap = mapGenerator.GenerateMap();
            for (var x = 0; x < CurrentMap.Width; x++)
            {
                for (var y = 0; y < CurrentMap.Height; y++)
                    switch (CurrentMap.GetValue(x, y))
                    {
                        case Map.Map.Empty:
                            break;

                        case Map.Map.Wall:
                            Instantiate(square, CurrentMap.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;

                        case Map.Map.Exit:
                            Instantiate(exit, CurrentMap.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;
                    }
            }

            var startRoom = CurrentMap.StartRoom;
            var exitRoom = CurrentMap.ExitRoom;

            #if UNITY_EDITOR
            if (Pathfinder.FindPath(CurrentMap, startRoom.Center, exitRoom.Center, out var path))
            {
                for (var i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(CurrentMap.GetWorldPosition(path[i - 1].Position), CurrentMap.GetWorldPosition(path[i].Position), Color.green, 5f);
                }
            }
            #endif
        }

        void EndPlayerTurn()
        {
            IsPlayerTurn = false;
            timer = .1f;
        }
    }
}