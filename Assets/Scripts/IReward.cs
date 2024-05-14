using UnityEngine;
using WSP.Units;

namespace WSP
{
    public interface IReward
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }

        void Apply(Unit target);
    }
}