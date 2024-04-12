using System.Collections.Generic;
using UnityEngine;
using WSP.Map.Pathfinding;
using WSP.Units;
using WSP.Units.Player;

namespace WSP.Map
{
    public class Level
    {
        public Pathfinding.Map Map { get; }
        public UnitQueue Units { get; }
        public IPlayerUnitController Player { get; private set; }

        public List<ILevelObject> Objects { get; } = new();
        public List<IInteractable> Interactables { get; } = new();

        public Level(Pathfinding.Map map)
        {
            Map = map;
            Units = new UnitQueue();
        }

        public void SetPlayer(IPlayerUnitController player)
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

        public ILevelObject GetObjectAt(Vector2Int position)
        {
            for (var i = 0; i < Units.Count; i++)
            {
                if (Objects[i].GridPosition == position) return Objects[i];
            }

            return null;
        }
        
        public IUnit GetUnitAt(Vector2Int position)
        {
            for (var i = 0; i < Units.Count; i++)
            {
                if (Units[i].Unit.GridPosition == position) return Units[i].Unit;
            }

            return null;
        }
        
        public void SpawnInteractable(IInteractable interactable)
        {
            Interactables.Add(interactable);
            Objects.Add(interactable);
        }
    }
}