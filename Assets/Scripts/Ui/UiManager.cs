using UnityEngine;
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

        static IPlayerUnitController PlayerController => GameManager.CurrentLevel.Player;


        void Awake()
        {
            instance = this;

            Canvas = GetComponent<Canvas>();
        }

        public void Initialize()
        {
            PlayerController.OnUnitXpGained += UpdateXpBar;
            xpBar.UpdateBar(PlayerController.Unit.Xp, PlayerController.Unit.XpToNextLevel);

            PlayerController.OnUnitHealthChanged += UpdateHealthBar;
            healthBar.UpdateBar(PlayerController.Unit.CurrentHealth, PlayerController.Unit.Stats.Health);

            PlayerController.OnUnitLevelUp += UpdateLevelCounter;
            UpdateLevelCounter(PlayerController.Unit.Level);

            PlayerController.OnOpenInventory += OpenInventory;
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

        void OpenInventory()
        {
            inventoryUi.Open();
        }
    }
}