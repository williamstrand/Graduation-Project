using System;

namespace WSP.Units
{
    public interface IUnitController
    {
        Action OnTurnEnd { get; set; }
        Unit Unit { get; }
        bool IsTurn { get; set; }

        void TurnStart();
        void SetUnit(Unit unit);
    }
}