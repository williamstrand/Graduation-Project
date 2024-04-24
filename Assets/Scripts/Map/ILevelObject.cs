using UnityEngine;

namespace WSP.Map
{
    public interface ILevelObject
    {
        Vector2Int GridPosition { get; }
        bool IsVisible { get; }

        void SetVisibility(bool visible);
        void Destroy();
    }
}