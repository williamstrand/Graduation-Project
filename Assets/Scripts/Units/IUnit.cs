using System;
using UnityEngine;

namespace WSP.Units
{
    public interface IUnit
    {
        Action OnDeath { get; set; }
        Action OnActionFinished { get; set; }
        Vector2Int GridPosition { get; }
        int CurrentHealth { get; }
        GameObject GameObject { get; }
        Stats Stats { get; }

        bool MoveTo(Vector2 position);
        void Attack(IUnit target);
        void Damage(int damage);
    }
}