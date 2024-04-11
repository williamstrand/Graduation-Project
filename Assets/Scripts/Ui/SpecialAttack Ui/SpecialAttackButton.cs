using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using WSP.Units;

namespace WSP.Ui
{
    public class SpecialAttackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image icon;
        [SerializeField] Image cooldownOverlay;
        [SerializeField] TextMeshProUGUI cooldownText;

        [SerializeField] ActionTextPopup actionTextPopup;

        static AssetLoader<Sprite> SpriteLoader;

        IAction currentAction;

        void Awake()
        {
            SpriteLoader ??= new AssetLoader<Sprite>(Constants.IconBundle, Constants.EmptyIcon);
        }

        public void Set(IAction action)
        {
            if (action == null)
            {
                currentAction = null;
                icon.sprite = SpriteLoader.LoadAsset(Constants.EmptyIcon);
                
                cooldownOverlay.enabled = false;
                cooldownText.enabled = false;
                return;
            }

            icon.sprite = action == currentAction ? icon.sprite : action.Icon;
            currentAction = action;

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