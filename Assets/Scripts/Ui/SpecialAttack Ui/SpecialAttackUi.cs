using UnityEngine;
using WSP.Units;

namespace WSP.Ui
{
    public class SpecialAttackUi : MonoBehaviour
    {
        [SerializeField] SpecialAttackButton[] specialAttackButtons;

        public void UpdateUi(IAction[] specialAttacks)
        {
            for (var i = 0; i < specialAttacks.Length; i++)
            {
                specialAttackButtons[i].Set(specialAttacks[i]);
            }
        }
    }
}