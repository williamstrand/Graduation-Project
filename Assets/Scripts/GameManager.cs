using System.Collections;
using UnityEngine;
using WSP.Map;
using WSP.Map.Pathfinding;
using WSP.Ui;
using WSP.Units;
using WSP.Units.Player;

namespace WSP
{
    public class GameManager : MonoBehaviour
    {
        public static Level CurrentLevel { get; private set; }
        static GameManager instance;

        [SerializeField] GameObject square;
        [SerializeField] GameObject exit;
        Transform mapParent;
        MapGenerator mapGenerator;

        IPlayerUnitController playerController;
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] Unit playerUnit;
        [SerializeField] LevelUpManager levelUpManager;
        [SerializeField] UiManager uiManager;

        void Awake()
        {
            instance = this;
            mapParent = new GameObject("Map").transform;
            GenerateMap();

            playerController = Instantiate(playerPrefab);
            var unit = Instantiate(playerUnit, CurrentLevel.Map.GetWorldPosition(CurrentLevel.Map.StartRoom.Center), Quaternion.identity);
            playerController.SetUnit(unit);
            CurrentLevel.Units.Enqueue(playerController);
            CurrentLevel.SetPlayer(playerController);

            levelUpManager.SetPlayer(playerController);
            uiManager.SetPlayer(playerController);
        }

        void Start()
        {
            StartTurn(playerController);
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

            var map = mapGenerator.GenerateMap();
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                    switch (map.GetValue(x, y))
                    {
                        case Map.Pathfinding.Map.Empty:
                            break;

                        case Map.Pathfinding.Map.Wall:
                            Instantiate(square, map.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;

                        case Map.Pathfinding.Map.Exit:
                            Instantiate(exit, map.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;
                    }
            }

            var startRoom = map.StartRoom;
            var exitRoom = map.ExitRoom;

            CurrentLevel = new Level(map);

            #if UNITY_EDITOR
            if (Pathfinder.FindPath(map, startRoom.Center, exitRoom.Center, out var path))
            {
                for (var i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(map.GetWorldPosition(path[i - 1].Position),
                        map.GetWorldPosition(path[i].Position),
                        Color.green,
                        5f);
                }
            }
            #endif
        }

        void StartTurn(IUnitController unitController)
        {
            unitController.IsTurn = true;
            unitController.TurnStart();
            unitController.OnTurnEnd += EndCurrentTurn;
        }

        void EndCurrentTurn()
        {
            CurrentLevel.Units.Peek().IsTurn = false;
            CurrentLevel.Units.Peek().OnTurnEnd -= EndCurrentTurn;

            CurrentLevel.Units.Enqueue(CurrentLevel.Units.Dequeue());

            StartTurn(CurrentLevel.Units.Peek());
        }

        public static Coroutine ExecuteCoroutine(IEnumerator routine)
        {
            return instance.StartCoroutine(routine);
        }
    }
}