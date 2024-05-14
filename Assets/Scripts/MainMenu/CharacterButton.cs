using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WSP.Units;

namespace WSP.MainMenu
{
    public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Action<CharacterButton> OnSelected { get; set; }

        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;

        [SerializeField] GameObject selectedIndicator;

        public Character Character { get; private set; }

        public void Set(Character character)
        {
            Character = character;

            image.sprite = character.Sprite;
            nameText.text = character.Name;
            descriptionText.text = character.Description;

            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            selectedIndicator.SetActive(selected);
        }

        public void OnPointerEnter(PointerEventData eventData) { }
        public void OnPointerExit(PointerEventData eventData) { }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }
    }
}