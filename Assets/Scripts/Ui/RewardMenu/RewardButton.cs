using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WSP.Ui.RewardMenu
{
    public class RewardButton : Button
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Image icon;

        public void SetReward(IReward reward)
        {
            nameText.text = reward.Name;
            descriptionText.text = reward.Description;
            icon.sprite = reward.Icon;
        }
    }
}