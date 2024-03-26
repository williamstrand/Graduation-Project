using UnityEngine;

namespace WSP.Units.Components
{
    public class SpecialAttackComponent : MonoBehaviour, ISpecialAttackComponent
    {
        public IAction[] SpecialAttacks { get; private set; }
        public IAction this[int index] => SpecialAttacks[Mathf.Clamp(index, 0, maxSpecialAttacks - 1)];

        [SerializeField] int maxSpecialAttacks = 4;

        void Awake()
        {
            SpecialAttacks = new IAction[maxSpecialAttacks];
        }

        public void SetSpecialAttack(int index, IAction specialAttack)
        {
            SpecialAttacks[Mathf.Clamp(index, 0, maxSpecialAttacks - 1)] = specialAttack;
        }
    }
}