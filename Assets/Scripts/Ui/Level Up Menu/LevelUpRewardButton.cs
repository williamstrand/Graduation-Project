using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WSP.Units.Upgrades;

namespace WSP.Ui
{
    public class LevelUpRewardButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI upgradeName;
        [SerializeField] TextMeshProUGUI upgradeDescription;
        [SerializeField] Image upgradeIcon;

        public Action OnClick { get; set; }

        void Awake()
        {
            ShowText(false);
        }

        public void SetUpgrade(Upgrade upgrade)
        {
            upgradeName.text = upgrade.Name;
            upgradeDescription.text = upgrade.Description;
            upgradeIcon.sprite = upgrade.Icon;
        }

        void ShowText(bool show)
        {
            upgradeDescription.gameObject.SetActive(show);
            upgradeName.gameObject.SetActive(show);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowText(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ShowText(false);
        }
    }
}