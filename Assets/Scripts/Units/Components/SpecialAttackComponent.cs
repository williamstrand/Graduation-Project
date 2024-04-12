using System;
using UnityEngine;

namespace WSP.Units.Components
{
    public class SpecialAttackComponent : MonoBehaviour, ISpecialAttackComponent
    {
        public Action<IAction[]> OnSpecialAttacksChanged { get; set; }
        public IAction[] SpecialAttacks { get; private set; }
        public IAction this[int index] => SpecialAttacks[Mathf.Clamp(index, 0, maxSpecialAttacks - 1)];

        [SerializeField] int maxSpecialAttacks = 4;

        IUnit unit;

        void Awake()
        {
            unit = GetComponent<IUnit>();
            SpecialAttacks = new IAction[maxSpecialAttacks];
        }

        void OnEnable()
        {
            unit.OnActionFinished += PlayerActionFinished;
        }

        void OnDisable()
        {
            unit.OnActionFinished -= PlayerActionFinished;
        }

        public void SetSpecialAttack(int index, IAction specialAttack)
        {
            SpecialAttacks[Mathf.Clamp(index, 0, maxSpecialAttacks - 1)] = specialAttack;
            specialAttack.Stats = unit.Stats;
            OnSpecialAttacksChanged?.Invoke(SpecialAttacks);
        }

        void PlayerActionFinished(IAction action)
        {
            if (action is IMovementComponent) return;

            UpdateCooldowns();
        }

        void UpdateCooldowns()
        {
            foreach (var specialAttack in SpecialAttacks)
            {
                if (specialAttack == null) continue;

                specialAttack.CooldownRemaining = Mathf.Max(0, specialAttack.CooldownRemaining - 1);
            }

            OnSpecialAttacksChanged?.Invoke(SpecialAttacks);
        }
    }
}