using System;
using UnityEngine;
using WSP.Ui.Inventory;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Ui
{
    [RequireComponent(typeof(Canvas))]
    public class UiManager : MonoBehaviour
    {
        public static Canvas Canvas { get; private set; }

        public Action<IReward> OnRewardSelected
        {
            get => rewardMenu.OnRewardSelected;
            set => rewardMenu.OnRewardSelected = value;
        }

        [SerializeField] Bar healthBar;
        [SerializeField] InventoryUi inventoryUi;
        [SerializeField] SpecialAttackUi specialAttackUi;
        [SerializeField] RewardMenu.RewardMenu rewardMenu;

        PlayerController playerController;

        void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void Initialize(PlayerController player)
        {
            playerController = player;

            playerController.OnUnitHealthChanged += UpdateHealthBar;
            healthBar.UpdateBar(playerController.Unit.CurrentHealth, playerController.Unit.Stats.Health);

            playerController.OnOpenInventory += OpenInventory;

            playerController.Unit.SpecialAttack.OnSpecialAttacksChanged += UpdateSpecialAttackUi;

            UpdateSpecialAttackUi(playerController.Unit.SpecialAttack.SpecialAttacks);
        }

        void UpdateHealthBar(float health, float maxHealth)
        {
            healthBar.UpdateBar(health, maxHealth);
        }

        void OpenInventory()
        {
            if (!inventoryUi.IsOpen)
            {
                inventoryUi.Open();
            }
            else
            {
                inventoryUi.Close();
            }
        }

        void UpdateSpecialAttackUi(IAction[] specialAttacks)
        {
            specialAttackUi.UpdateUi(specialAttacks);
        }

        public void OpenRewardScreen()
        {
            rewardMenu.Open();

            rewardMenu.OnRewardSelected += RewardSelected;
            return;

            void RewardSelected(IReward reward)
            {
                rewardMenu.Close();
            }
        }
    }
}