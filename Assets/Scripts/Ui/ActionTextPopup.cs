using TMPro;
using UnityEngine;
using WSP.Units;

namespace WSP.Ui
{
    public class ActionTextPopup : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] TextMeshProUGUI cooldownText;

        public void Open(bool open)
        {
            gameObject.SetActive(open);
        }

        public void SetAction(IAction action)
        {
            nameText.text = action.Name;
            descriptionText.text = action.Description;
            cooldownText.text = action.Cooldown.ToString();
        }
    }
}