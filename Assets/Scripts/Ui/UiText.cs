using TMPro;
using UnityEngine;

namespace WSP.Ui
{
    public class UiText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelText;

        public void UpdateText(string text)
        {
            levelText.text = text;
        }
    }
}