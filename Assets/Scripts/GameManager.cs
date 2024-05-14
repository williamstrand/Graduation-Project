using System;
using System.Collections;
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
        [SerializeField] LevelExit levelExitPrefab;
        Transform mapParent;
        MapGenerator mapGenerator;
        EnemySpawner enemySpawner;

        PlayerController playerController;
        [SerializeField] PlayerController playerPrefab;
        [SerializeField] UnitController enemyPrefab;
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
            var character = ScriptableObject.CreateInstance<Character>();
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Character"), character);
            var playerUnit = character.Unit;
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
            CurrentLevel.OnExit += ExitLevel;
            tilemap.ClearAllTiles();
            fogOfWar.Clear();

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

            enemySpawner = new EnemySpawner(10, 5, enemyPrefab, CurrentLevel);
            onTurnEnd += enemySpawner.SpawnEnemies;

            var exitPosition = map.GetWorldPosition(map.ExitRoom.GetRandomPosition());
            var exit = Instantiate(levelExitPrefab, exitPosition, Quaternion.identity, mapParent);
            CurrentLevel.AddInteractable(exit);
            CurrentLevel.FogOfWar = fogOfWar;

            playerController.Unit.Movement.SetPosition(CurrentLevel.Map.StartRoom.Center);
            playerController.OnTurnStart = null;
            playerController.OnTurnEnd = null;

            const int playerVisibility = 4;
            playerController.OnTurnStart += () => fogOfWar.SetArea(map, playerController.Unit.GridPosition, playerVisibility, true);
            playerController.OnTurnEnd += () => fogOfWar.SetArea(map, playerController.Unit.GridPosition, playerVisibility, true);
            CurrentLevel.SetPlayer(playerController);
            CameraController.ForceSetPosition(playerController.Unit.GridPosition);
        }

        void ExitLevel()
        {
            GenerateLevel();
            StartCoroutine(ExitLevelCoroutine());
        }

        IEnumerator ExitLevelCoroutine()
        {
            var open = true;

            uiManager.OpenRewardScreen();
            uiManager.OnRewardSelected += GetReward;
            while (open)
            {
                yield return null;
            }

            StartTurn(playerController);

            yield break;

            void GetReward(IReward reward)
            {
                uiManager.OnRewardSelected -= GetReward;
                open = false;
                reward.Apply(playerController.Unit);
            }
        }

        void StartTurn(IUnitController unitController)
        {
            unitController.IsTurn = true;
            unitController.TurnStart();
            unitController.OnTurnEnd += EndCurrentTurn;

            if (ReferenceEquals(unitController, playerController))
            {
                onTurnEnd?.Invoke();
            }

            UpdateVisibility();
        }

        void EndCurrentTurn()
        {
            if (CurrentLevel.Units.Count == 0) return;

            CurrentLevel.Units.Peek().IsTurn = false;
            CurrentLevel.Units.Peek().OnTurnEnd -= EndCurrentTurn;

            CurrentLevel.Units.Enqueue(CurrentLevel.Units.Dequeue());

            StartTurn(CurrentLevel.Units.Peek());
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