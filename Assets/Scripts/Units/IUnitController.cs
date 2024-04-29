using System;

namespace WSP.Units
{
    public interface IUnitController
    {
        Action OnTurnStart { get; set; }
        Action OnTurnEnd { get; set; }
        Unit Unit { get; }
        bool IsTurn { get; set; }

        void TurnStart();
        void SetUnit(Unit unit);
        bool StartAction(ActionContext action);
        void Destroy();
    }
}