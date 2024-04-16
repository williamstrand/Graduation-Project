using UnityEngine;

namespace WSP.Map
{
    public interface ILevelObject
    {
        Vector2Int GridPosition { get; }

        void Destroy();
    }
}