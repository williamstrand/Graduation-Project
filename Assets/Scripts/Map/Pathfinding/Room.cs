using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map.Pathfinding
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

        /// <summary>
        ///     Checks if the room is connected to another room.
        /// </summary>
        /// <param name="otherRoom">the other room.</param>
        /// <returns>true if the rooms are connected.</returns>
        public bool IsConnected(Room otherRoom)
        {
            return ConnectedRooms.Contains(otherRoom);
        }

        /// <summary>
        ///     Connect the room to another room.
        /// </summary>
        /// <param name="otherRoom">the other room.</param>
        public void Connect(Room otherRoom)
        {
            if (!IsConnected(otherRoom))
            {
                ConnectedRooms.Add(otherRoom);
            }
        }

        /// <summary>
        ///     Gets the closest room from a list of rooms.
        /// </summary>
        /// <param name="rooms">the list of rooms.</param>
        /// <returns>the closest room.</returns>
        public Room GetClosestRoom(List<Room> rooms)
        {
            Room closestRoom = null;
            var closestDistance = float.MaxValue;

            for (var i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] == this) continue;
                if (IsConnected(rooms[i])) continue;

                var distance = Vector2Int.Distance(rooms[i].Center, Center);
                if (!(distance < closestDistance)) continue;

                closestDistance = distance;
                closestRoom = rooms[i];
            }

            return closestRoom;
        }

        /// <summary>
        ///     Checks if the room overlaps with any other room from a list.
        /// </summary>
        /// <param name="rooms">the list of rooms to check.</param>
        /// <param name="wallSize">the required size of the walls between room.</param>
        /// <returns>true if any room overlaps with this room.</returns>
        public bool CheckForOverlap(List<Room> rooms, int wallSize)
        {
            for (var i = 0; i < rooms.Count; i++)
            {
                if (rooms[i] == null) break;

                if (CheckForOverlap(rooms[i], wallSize)) return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks if the room overlaps with another room.
        /// </summary>
        /// <param name="room">the other room.</param>
        /// <param name="wallSize">the required size of the walls between room.</param>
        /// <returns>true if the rooms overlap.</returns>
        public bool CheckForOverlap(Room room, int wallSize)
        {
            return room.BottomLeft.x - wallSize <= TopRight.x &&
                   room.TopRight.x + wallSize >= BottomLeft.x &&
                   room.BottomLeft.y - wallSize <= TopRight.y &&
                   room.TopRight.y + wallSize >= BottomLeft.y;
        }
        
        public Vector2Int GetRandomPosition()
        {
            return new Vector2Int(Random.Range(BottomLeft.x, TopRight.x), Random.Range(BottomLeft.y, TopRight.y));
        }
    }
}