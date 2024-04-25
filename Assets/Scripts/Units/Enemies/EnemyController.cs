using UnityEngine;
using Utility;
using WSP.Items;
using WSP.Map;
using WSP.Map.Pathfinding;

namespace WSP.Units.Enemies
{
    public class EnemyController : UnitController
    {
        const float ItemChance = .1f;
        const int VisionRange = 7;

        [SerializeField] Unit unitPrefab;

        Vector2Int targetPosition;
        IUnit player;

        void Awake()
        {
            SetUnit(Instantiate(unitPrefab));

            if (Random.value <= ItemChance)
            {
                var randomItem = ItemDatabase.AllDroppableItems[Random.Range(0, Unit.Inventory.Amount)];
                Unit.Inventory.AddItem(randomItem);
            }
        }

        void Update()
        {
            if (Unit == null) return;
            if (!IsTurn) return;
            if (Unit.ActionInProgress) return;

            ActionContext targetAction;
            if (targetPosition == Vector2Int.zero)
            {
                targetPosition = GameManager.CurrentLevel.Map.Rooms[Random.Range(0, GameManager.CurrentLevel.Map.Rooms.Count)].GetRandomPosition();
            }

            if (player == null)
            {
                if (Pathfinder.Distance(Unit.GridPosition, GameManager.CurrentLevel.Player.Unit.GridPosition) < VisionRange)
                {
                    player = GameManager.CurrentLevel.Player.Unit;
                    targetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
                }
                else
                {
                    if (targetPosition == Unit.GridPosition)
                    {
                        targetPosition = GameManager.CurrentLevel.Map.Rooms[Random.Range(0, GameManager.CurrentLevel.Map.Rooms.Count)].GetRandomPosition();
                    }

                    targetAction = GetAction(targetPosition);
                }
            }
            else
            {
                targetAction = GetAction(GameManager.CurrentLevel.Player.Unit.GridPosition);
            }

            if (!StartAction(targetAction))
            {
                EndTurn();
            }
        }

        ActionContext GetAction(Vector2Int gridPosition)
        {
            if (player == null)
            {
                return new ActionContext(Unit.Movement, gridPosition);
            }

            if (Pathfinder.Distance(Unit.GridPosition, gridPosition) > Unit.Stats.AttackRange)
            {
                return new ActionContext(Unit.Movement, gridPosition);
            }

            return new ActionContext(Unit.Attack, player.GridPosition);
        }

        protected override void Kill()
        {
            if (Unit.GameObject == null) return;

            if (GameManager.CurrentLevel.GetObjectAt(Unit.GridPosition) is not IInteractable)
            {
                var inventory = Unit.Inventory;
                var amount = inventory.Amount;
                var randomItem = inventory[Random.Range(0, amount)];

                var itemDropPrefab = new AssetLoader<ItemDrop>("level").LoadAsset("Item Drop");

                if (randomItem == null) return;

                var itemDrop = Instantiate(itemDropPrefab, GameManager.CurrentLevel.Map.GetWorldPosition(Unit.GridPosition), Quaternion.identity);
                itemDrop.SetItem(randomItem);
                itemDrop.GridPosition = Unit.GridPosition;

                GameManager.CurrentLevel.AddInteractable(itemDrop);
            }

            Destroy(Unit.GameObject);
            GameManager.CurrentLevel.RemoveUnit(this);
            Unit = null;
            Destroy(gameObject);
        }
    }
}