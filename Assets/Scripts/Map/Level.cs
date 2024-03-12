using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Units;

namespace WSP.Map
{
    public class Level
    {
        public Pathfinding.Map Map { get; }
        public UnitQueue Units { get; }
        public IUnit Player { get; private set; }

        public Level(Pathfinding.Map map)
        {
            Map = map;
            Units = new UnitQueue();
        }

        public void SetPlayer(IUnit player)
        {
            Player = player;
        }

        public bool FindPath(Vector2Int start, Vector2Int target, out Path path)
        {
            var tempMap = Map.Copy();
            for (var i = 0; i < Units.Count; i++)
            {
                if (Units[i].Unit.GridPosition == start) continue;
                if (Units[i].Unit.GridPosition == target) continue;

                tempMap.SetValue(Units[i].Unit.GridPosition, Pathfinding.Map.Wall);
            }

            return Pathfinder.FindPath(tempMap, start, target, out path);
        }

        public bool IsOccupied(Vector2Int position)
        {
            for (var i = 0; i < Units.Count; i++)
            {
                if (Units[i].Unit.GridPosition == position) return true;
            }

            return false;
        }

        public IUnit GetUnitAt(Vector2Int position)
        {
            for (var i = 0; i < Units.Count; i++)
            {
                if (Units[i].Unit.GridPosition == position) return Units[i].Unit;
            }

            return null;
        }
    }
}