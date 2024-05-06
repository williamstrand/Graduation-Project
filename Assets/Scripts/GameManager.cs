using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using WSP.Camera;
using WSP.Map;
using WSP.Ui;
using WSP.Units;
using WSP.Units.Enemies;
using WSP.Units.Player;

namespace WSP
{
    public class GameManager : MonoBehaviour
    {
        public static Level CurrentLevel { get; private set; }

        Action onTurnEnd;

        [SerializeField] TileBase wall;
        [SerializeField] GameObject ground;
        [SerializeField] Exit exitPrefab;
        Transform mapParent;
        MapGenerator mapGenerator;
        EnemySpawner enemySpawner;

        [SerializeField] TextMeshProUGUI turnText;

        PlayerController playerController;
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] UnitController enemyPrefab;
        [SerializeField] Unit playerUnit;
        [SerializeField] UiManager uiManager;
        [SerializeField] FogOfWar fogOfWar;

        [SerializeField] Tilemap tilemap;

        void Awake()
        {
            mapParent = new GameObject("Map").transform;
        }

        void Start()
        {
            fogOfWar.OnFogChange += UpdateVisibility;
            StartGame();
        }

        void StartGame()
        {
            playerController = Instantiate(playerPrefab);
            playerController.SetUnit(Instantiate(playerUnit));

            GenerateLevel();

            uiManager.Initialize();

            UpdateVisibility();

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

            CurrentLevel?.Clean();
            CurrentLevel = new Level(map);

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    switch (map.GetValue(x, y))
                    {
                        case Map.Pathfinding.Map.Empty:
                            Instantiate(ground, map.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;

                        case Map.Pathfinding.Map.Wall:
                            tilemap.SetTile(new Vector3Int(x, y), wall);
                            Instantiate(ground, map.GetWorldPosition(x, y), Quaternion.identity, mapParent);
                            break;
                    }

                    fogOfWar.Set(new Vector2Int(x, y), false);
                }
            }

            onTurnEnd = null;

            enemySpawner = new EnemySpawner(3, 5, enemyPrefab, CurrentLevel);
            onTurnEnd += enemySpawner.SpawnEnemies;

            var exitPosition = map.GetWorldPosition(map.ExitRoom.GetRandomPosition());
            var exit = Instantiate(exitPrefab, exitPosition, Quaternion.identity, mapParent);
            exit.OnInteract += ExitLevel;
            CurrentLevel.AddInteractable(exit);
            CurrentLevel.FogOfWar = fogOfWar;

            playerController.Unit.Movement.SetPosition(CurrentLevel.Map.StartRoom.Center);
            fogOfWar.SetArea(map, CurrentLevel.Map.StartRoom.Center, 4, true);
            playerController.OnTurnStart += () => fogOfWar.SetArea(map, playerController.Unit.GridPosition, 4, true);
            playerController.OnTurnEnd += () => fogOfWar.SetArea(map, playerController.Unit.GridPosition, 4, true);
            CurrentLevel.SetPlayer(playerController);
            CameraController.ForceSetPosition(playerController.Unit.GridPosition);
        }

        void StartTurn(IUnitController unitController)
        {
            UpdateVisibility();
            turnText.text = ReferenceEquals(unitController, playerController) ? "Player Turn" : "Enemy Turn";

            unitController.IsTurn = true;
            unitController.TurnStart();
            unitController.OnTurnEnd += EndCurrentTurn;

            if (ReferenceEquals(unitController, playerController))
            {
                onTurnEnd?.Invoke();
            }
        }

        void EndCurrentTurn()
        {
            if (CurrentLevel.Units.Count == 0) return;

            CurrentLevel.Units.Peek().IsTurn = false;
            CurrentLevel.Units.Peek().OnTurnEnd -= EndCurrentTurn;

            CurrentLevel.Units.Enqueue(CurrentLevel.Units.Dequeue());

            StartTurn(CurrentLevel.Units.Peek());
        }

        void ExitLevel()
        {
            GenerateLevel();
        }

        void UpdateVisibility()
        {
            for (var i = 0; i < CurrentLevel.Objects.Count; i++)
            {
                var levelObject = CurrentLevel.Objects[i];
                var isVisible = !fogOfWar.IsHidden(levelObject.GridPosition);
                levelObject.SetVisibility(isVisible);
            }
        }
    }
}