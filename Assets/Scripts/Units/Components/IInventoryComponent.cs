using WSP.Items;

namespace WSP.Units.Components
{
    public interface IInventoryComponent
    {
        int Size { get; }
        int Amount { get; }

        Item this[int index] { get; }
        Item[] GetAllItems();
        bool AddItem(Item item);
    }
}