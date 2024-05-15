using System;
using System.Collections.Generic;
using UnityEngine;
using WSP.Items;
using WSP.Units.SpecialAttacks;
using WSP.Units.Upgrades;
using Random = UnityEngine.Random;

namespace WSP.Ui.RewardMenu
{
    public class RewardMenu : MonoBehaviour
    {
        const int RewardCount = 3;

        public Action<IReward> OnRewardSelected;

        RewardButton[] rewardButtons = new RewardButton[RewardCount];
        [SerializeField] RewardButton rewardButtonPrefab;
        [SerializeField] Transform buttonParent;

        List<IReward> rewards;

        void Awake()
        {
            Close();
        }

        void SelectReward(IReward reward)
        {
            OnRewardSelected?.Invoke(reward);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            foreach (var button in rewardButtons)
            {
                if (button == null) continue;

                Destroy(button.gameObject);
            }
        }

        public void Skip()
        {
            OnRewardSelected?.Invoke(null);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            SetRewards();
        }

        void SetRewards()
        {
            var type = Random.Range(0, 3);

            rewards = new List<IReward>(type switch
            {
                0 => SpecialAttackDatabase.AllSpecialAttacks,
                1 => ItemDatabase.AllDroppableItems,
                2 => UpgradeDatabase.AllUpgrades,
                _ => Array.Empty<IReward>()
            });

            for (var i = 0; i < RewardCount; i++)
            {
                if (rewards.Count == 0) break;

                rewardButtons[i] = Instantiate(rewardButtonPrefab, buttonParent);
                var reward = rewards[Random.Range(0, rewards.Count)];
                rewards.Remove(reward);

                rewardButtons[i].OnClick = () => SelectReward(reward);
                rewardButtons[i].SetReward(reward);
            }
        }
    }
}