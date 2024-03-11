using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map.Pathfinding
{
    public class Path
    {
        public Node this[int index] => nodes[index];
        public int Count => nodes.Count;

        List<Node> nodes = new();

        public void Add(Node node)
        {
            nodes.Add(node);
        }

        public void Reverse()
        {
            nodes.Reverse();
        }

        public List<Vector2Int> ToVector2Int()
        {
            var list = new List<Vector2Int>();
            for (var i = 0; i < nodes.Count; i++)
            {
                list.Add(nodes[i]);
            }

            return list;
        }
    }
}