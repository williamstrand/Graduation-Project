using System;

namespace WSP.Units
{
    public interface IUnitController
    {
        Action OnTurnEnd { get; set; }
        IUnit Unit { get; }
        bool IsTurn { get; set; }

        void TurnStart();
        void SetUnit(IUnit unit);
        bool StartAction(ActionContext action);
    }
}