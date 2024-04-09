using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WSP.Units;

namespace WSP.Ui
{
    public class SpecialAttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image icon;
        [SerializeField] Image cooldownOverlay;
        [SerializeField] TextMeshProUGUI cooldownText;

        [SerializeField] ActionTextPopup actionTextPopup;

        IAction currentAction;

        public void Set(IAction action)
        {
            currentAction = action;

            if (action == null) return;

            icon.sprite = action.Icon;

            if (action.CooldownRemaining > 0)
            {
                cooldownOverlay.enabled = true;
                cooldownText.text = action.CooldownRemaining.ToString();
                cooldownText.enabled = true;
                return;
            }

            cooldownOverlay.enabled = false;
            cooldownText.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentAction == null) return;

            actionTextPopup.SetAction(currentAction);
            actionTextPopup.Open(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            actionTextPopup.Open(false);
        }
    }
}