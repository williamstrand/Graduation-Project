using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map.Pathfinding
{
    public static class Pathfinder
    {
        static readonly float SQRT2 = Mathf.Sqrt(2);
        static List<Node> open = new();
        static List<Node> closed = new();

        /// <summary>
        ///     Find a path from start to end.
        /// </summary>
        /// <param name="map">the map to path find on.</param>
        /// <param name="start">the start position.</param>
        /// <param name="end">the end position.</param>
        /// <param name="path">the path.</param>
        /// <returns>true if a path could be found.</returns>
        public static bool FindPath(Map map, Vector2Int start, Vector2Int end, out Path path)
        {
            open.Clear();
            closed.Clear();

            open.Add(new Node(start));

            while (open.Count != 0)
            {
                var q = GetLowestFCostNode();
                open.Remove(q);

                if (q.Position == end)
                {
                    path = CreatePath(q);
                    return true;
                }

                var neighbours = map.GetNeighbours(q.Position);
                for (var i = 0; i < neighbours.Count; i++)
                {
                    if (InList(open, neighbours[i])) continue;
                    if (InList(closed, neighbours[i])) continue;

                    var value = map.GetValue(neighbours[i]);
                    if (value == Map.Wall) continue;

                    var node = new Node(neighbours[i]);
                    node.Parent = q;

                    if (node.Position == end)
                    {
                        path = CreatePath(node);
                        return true;
                    }

                    open.Add(node);
                    node.GCost = q.GCost + 1;
                    node.HCost = GetDistance(node.Position, end);
                }

                closed.Add(q);
            }

            path = default;
            return false;
        }

        /// <summary>
        ///     Checks if a node with the specified position exist in list.
        /// </summary>
        /// <param name="list">the list to check in.</param>
        /// <param name="position">the position to check for.</param>
        /// <returns>true if it exists in the list.</returns>
        static bool InList(List<Node> list, Vector2Int position)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Position == position) return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets the distance between point a and b
        /// </summary>
        /// <param name="a">first point.</param>
        /// <param name="b">second point.</param>
        /// <returns>the distance as an int.</returns>
        static int GetDistance(Vector2Int a, Vector2Int b)
        {
            var x = Mathf.Abs(a.x - b.x);
            var y = Mathf.Abs(a.y - b.y);

            return x + y;
        }

        /// <summary>
        ///     Creates a path from the first node to the final node.
        /// </summary>
        /// <param name="finalNode">the final node.</param>
        /// <returns>a path.</returns>
        static Path CreatePath(Node finalNode)
        {
            var path = new Path();
            var node = finalNode;
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        ///     Gets the node with the lowest FCost in the list of open nodes.
        /// </summary>
        /// <returns></returns>
        static Node GetLowestFCostNode()
        {
            var lowestFCost = int.MaxValue;
            Node lowestFCostNode = null;

            for (var i = 0; i < open.Count; i++)
            {
                if (open[i].FCost >= lowestFCost) continue;

                lowestFCost = open[i].FCost;
                lowestFCostNode = open[i];
            }

            return lowestFCostNode;
        }

        public static int Distance(Vector2Int start, Vector2Int target)
        {
            var deltaX = Mathf.Abs(start.x - target.x);
            var deltaY = Mathf.Abs(start.y - target.y);

            var min = Mathf.Min(deltaX, deltaY);
            var max = Mathf.Max(deltaX, deltaY);

            var diagonalSteps = min;
            var straightSteps = max - min;

            return (int)(SQRT2 * diagonalSteps + straightSteps);
        }
    }
}