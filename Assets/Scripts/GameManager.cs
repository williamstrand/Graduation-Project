using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using WSP.Camera;
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
        [FormerlySerializedAs("exit")][SerializeField] Exit exitPrefab;
        Transform mapParent;
        MapGenerator mapGenerator;

        IPlayerUnitController playerController;
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] Unit playerUnit;
        [SerializeField] UiManager uiManager;

        void Awake()
        {
            instance = this;
            mapParent = new GameObject("Map").transform;
        }

        void Start()
        {
            Invoke(nameof(StartGame), 2);
        }
        
        void StartGame()
        {
            GenerateLevel();
            playerController = Instantiate(playerPrefab);
            playerController.SetUnit(Instantiate(playerUnit));
            CurrentLevel.Units.Enqueue(playerController);
            CurrentLevel.Objects.Add(playerController.Unit);
            playerController.Unit.Movement.SetPosition(CurrentLevel.Map.StartRoom.Center);
            CurrentLevel.SetPlayer(playerController);
            CameraController.ForceSetPosition(playerController.Unit.GridPosition);
            
            uiManager.Initialize();
            
            StartTurn(playerController);
        }

        void GenerateLevel()
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
                    }
            }
            CurrentLevel = new Level(map);
            
            var exitPosition = map.GetWorldPosition(map.ExitRoom.GetRandomPosition());
            var exit = Instantiate(exitPrefab, exitPosition, Quaternion.identity, mapParent);
            exit.OnExit += GenerateLevel;
            CurrentLevel.SpawnInteractable(exit);
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