using WSP.Units;

namespace WSP.Map
{
    public interface IInteractable : ILevelObject
    {
        bool Interact(IUnit unit);
    }
}