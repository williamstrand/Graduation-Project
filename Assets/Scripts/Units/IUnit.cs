using System;

namespace WSP.Units
{
    public interface IUnit
    {
        Action OnTurnEnd { get; set; }
    }
}