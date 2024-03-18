using System;

namespace WSP.Units
{
    [Serializable]
    public class Stats
    {
        public float Health = 1;
        public float Attack = 1;
        public float PhysicalDefense;
        public float MagicDefense;
        public float AttackRange = 1;

        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats
            {
                Health = a.Health + b.Health,
                Attack = a.Attack + b.Attack,
                PhysicalDefense = a.PhysicalDefense + b.PhysicalDefense,
                MagicDefense = a.MagicDefense + b.MagicDefense,
                AttackRange = a.AttackRange + b.AttackRange
            };
        }

        public static Stats operator -(Stats a, Stats b)
        {
            return new Stats
            {
                Health = a.Health - b.Health,
                Attack = a.Attack - b.Attack,
                PhysicalDefense = a.PhysicalDefense - b.PhysicalDefense,
                MagicDefense = a.MagicDefense - b.MagicDefense,
                AttackRange = a.AttackRange - b.AttackRange
            };
        }

        public static Stats operator *(Stats a, Stats b)
        {
            return new Stats
            {
                Health = a.Health * b.Health,
                Attack = a.Attack * b.Attack,
                PhysicalDefense = a.PhysicalDefense * b.PhysicalDefense,
                MagicDefense = a.MagicDefense * b.MagicDefense,
                AttackRange = a.AttackRange * b.AttackRange
            };
        }

        public static Stats operator /(Stats a, Stats b)
        {
            return new Stats
            {
                Health = a.Health / b.Health,
                Attack = a.Attack / b.Attack,
                PhysicalDefense = a.PhysicalDefense / b.PhysicalDefense,
                MagicDefense = a.MagicDefense / b.MagicDefense,
                AttackRange = a.AttackRange / b.AttackRange
            };
        }
    }
}