using System;

namespace WSP.Units
{
    [Serializable]
    public class Stats
    {
        public float Health;
        public float Attack;
        public float MagicPower;
        public float PhysicalDefense;
        public float MagicDefense;
        public int AttackRange;

        public Stats(int startValues)
        {
            Health = startValues;
            Attack = startValues;
            PhysicalDefense = startValues;
            MagicDefense = startValues;
            AttackRange = startValues;
        }

        public Stats() { }

        // public static Stats operator +(Stats a, Stats b)
        // {
        //     return new Stats
        //     {
        //         Health = a.Health + b.Health,
        //         Attack = a.Attack + b.Attack,
        //         PhysicalDefense = a.PhysicalDefense + b.PhysicalDefense,
        //         MagicDefense = a.MagicDefense + b.MagicDefense,
        //         AttackRange = a.AttackRange + b.AttackRange
        //     };
        // }
        //
        // public static Stats operator -(Stats a, Stats b)
        // {
        //     return new Stats
        //     {
        //         Health = a.Health - b.Health,
        //         Attack = a.Attack - b.Attack,
        //         PhysicalDefense = a.PhysicalDefense - b.PhysicalDefense,
        //         MagicDefense = a.MagicDefense - b.MagicDefense,
        //         AttackRange = a.AttackRange - b.AttackRange
        //     };
        // }
        //
        // public static Stats operator *(Stats a, Stats b)
        // {
        //     return new Stats
        //     {
        //         Health = a.Health * b.Health,
        //         Attack = a.Attack * b.Attack,
        //         PhysicalDefense = a.PhysicalDefense * b.PhysicalDefense,
        //         MagicDefense = a.MagicDefense * b.MagicDefense,
        //         AttackRange = a.AttackRange * b.AttackRange
        //     };
        // }
        //
        // public static Stats operator /(Stats a, Stats b)
        // {
        //     return new Stats
        //     {
        //         Health = a.Health / b.Health,
        //         Attack = a.Attack / b.Attack,
        //         PhysicalDefense = a.PhysicalDefense / b.PhysicalDefense,
        //         MagicDefense = a.MagicDefense / b.MagicDefense,
        //         AttackRange = a.AttackRange / b.AttackRange
        //     };
        // }
    }
}