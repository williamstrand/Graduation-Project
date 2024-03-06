using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map
{
    public class Room
    {
        public Vector2Int BottomLeft;
        public Vector2Int TopRight;
        public Vector2Int Center => new((BottomLeft.x + TopRight.x) / 2, (BottomLeft.y + TopRight.y) / 2);

        public List<Room> ConnectedRooms = new();

        public Room(Vector2Int bottomLeft, Vector2Int topRight)
        {
            BottomLeft = bottomLeft;
            TopRight = topRight;
        }

        public bool IsConnected(Room otherRoom)
        {
            return ConnectedRooms.Contains(otherRoom);
        }

        public void Connect(Room otherRoom)
        {
            if (!IsConnected(otherRoom))
            {
                ConnectedRooms.Add(otherRoom);
            }
        }

        public Room GetClosestRoom(List<Room> rooms)
        {
            Room closestRoom = null;
            var closestDistance = float.MaxValue;

            for (var i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] == null) break;

                if (rooms[i] == this) continue;
                if (IsConnected(rooms[i])) continue;

                var distance = Vector2Int.Distance(rooms[i].Center, Center);
                if (!(distance < closestDistance)) continue;

                closestDistance = distance;
                closestRoom = rooms[i];
            }

            return closestRoom;
        }

        public bool CheckForOverlap(List<Room> rooms, int wallSize)
        {
            for (var i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] == null) break;

                if (CheckForOverlap(rooms[i], wallSize)) return true;
            }

            return false;
        }

        public bool CheckForOverlap(Room room, int wallSize)
        {
            return room.BottomLeft.x - wallSize <= TopRight.x &&
                   room.TopRight.x + wallSize >= BottomLeft.x &&
                   room.BottomLeft.y - wallSize <= TopRight.y &&
                   room.TopRight.y + wallSize >= BottomLeft.y;
        }
    }
}