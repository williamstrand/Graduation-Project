using UnityEngine;
using WSP.Units.Player;

namespace WSP.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiManager : MonoBehaviour
    {
        public static Canvas Canvas { get; private set; }

        [SerializeField] Bar healthBar;
        [SerializeField] Bar xpBar;
        [SerializeField] UiText levelCounter;

        IPlayerUnitController playerController;

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
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
    }
}