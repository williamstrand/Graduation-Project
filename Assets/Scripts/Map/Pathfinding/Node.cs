using UnityEngine;

namespace WSP.Map.Pathfinding
{
    public class Node
    {
        public Vector2Int Position { get; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public Node Parent { get; set; }

        public Node(Vector2Int position)
        {
            Position = position;
        }

        public static implicit operator Vector2Int(Node node)
        {
            return node.Position;
        }
    }
}