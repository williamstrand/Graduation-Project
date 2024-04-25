using System;
using WSP.Units;

namespace WSP.Map
{
    public interface IInteractable : ILevelObject
    {
        public Action OnInteract { get; set; }

        bool CanInteract(IUnit unit);
        bool Interact(IUnit unit);
    }
}