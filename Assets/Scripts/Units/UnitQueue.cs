using System.Collections.Generic;

namespace WSP.Units
{
    public class UnitQueue
    {
        List<IUnitController> list = new();
        public int Count => list.Count;

        public IUnitController this[int index] => list[index];

        public void Enqueue(IUnitController item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(IUnitController item)
        {
            return list.Contains(item);
        }

        public bool Remove(IUnitController item)
        {
            return list.Remove(item);
        }

        public IUnitController Dequeue()
        {
            var first = list[0];
            list.RemoveAt(0);
            return first;
        }

        public IUnitController Peek()
        {
            return list[0];
        }
    }
}