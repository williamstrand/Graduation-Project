using System.Collections.Generic;
using UnityEngine;
using WSP.Items;
using WSP.Ui.Inventory;
using WSP.Units.Player;

namespace WSP.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiManager : MonoBehaviour
    {
        public static Canvas Canvas { get; private set; }


        static UiManager instance;

        [SerializeField] Bar healthBar;
        [SerializeField] Bar xpBar;
        [SerializeField] UiText levelCounter;
        [SerializeField] InventoryUi inventoryUi;

        IPlayerUnitController playerController;

        List<IMenu> menus = new();

        void Awake()
        {
            instance = this;

            Canvas = GetComponent<Canvas>();

            menus.Add(inventoryUi);
        }

        public void SetPlayer(IPlayerUnitController player)
        {
            playerController = player;

            playerController.OnUnitXpGained += UpdateXpBar;
            xpBar.UpdateBar(playerController.Unit.Xp, playerController.Unit.XpToNextLevel);

            playerController.OnUnitHealthChanged += UpdateHealthBar;
            healthBar.UpdateBar(playerController.Unit.CurrentHealth, playerController.Unit.Stats.Health);

            playerController.OnUnitLevelUp += UpdateLevelCounter;
            UpdateLevelCounter(player.Unit.Level);

            playerController.OnOpenInventory += OpenInventory;
        }

        void UpdateHealthBar(float health, float maxHealth)
        {
            healthBar.UpdateBar(health, maxHealth);
        }

        void UpdateXpBar(float xp, float xpToNextLevel)
        {
            xpBar.UpdateBar(xp, xpToNextLevel);
        }

        void UpdateLevelCounter(int level)
        {
            levelCounter.UpdateText(level.ToString());
        }

        void OpenInventory(Item[] items)
        {
            inventoryUi.SetItems(items);
            inventoryUi.Open();
        }

        public static bool MenuOpen()
        {
            for (var i = 0; i < instance.menus.Count; i++)
            {
                if (instance.menus[i].IsOpen) return true;
            }

            return false;
        }
    }
}