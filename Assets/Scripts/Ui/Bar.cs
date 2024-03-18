using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WSP.Ui
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] Slider bar;
        [SerializeField] TextMeshProUGUI text;

        public void UpdateBar(float value, float maxValue)
        {
            bar.value = value / maxValue;
            text.text = $"{(int)value} / {(int)maxValue}";
        }
    }
}