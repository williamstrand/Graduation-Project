using System.Collections.Generic;
using UnityEngine;
using WSP.Map.Pathfinding;

namespace WSP.Map
{
    public class MapGenerator
    {
        const int MaxTries = 1000;

        public Vector2Int StartRoomWidth { get; set; }
        public Vector2Int StartRoomHeight { get; set; }
        public Vector2Int RoomWidth { get; set; }
        public Vector2Int RoomHeight { get; set; }
        public int MaxRooms { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float CellSize { get; set; }

        public int WallSize { get; set; } = 1;

        List<Room> rooms = new();
        Map map;

        /// <summary>
        ///     Generates a map. Set properties before calling this method.
        /// </summary>
        /// <returns>a Map.</returns>
        public Map GenerateMap()
        {
            map = new Map(Width, Height, CellSize);
            map.Rooms = rooms;
            rooms.Clear();

            // Fill grid with walls
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Width; y++) map.SetValue(x, y, Map.Wall);
            }

            // Generate start room
            var roomWidth = Random.Range(StartRoomWidth.x, StartRoomWidth.y);
            var roomHeight = Random.Range(StartRoomHeight.x, StartRoomHeight.y);
            var randomX = Random.Range(0, Width - roomWidth);
            var randomY = Random.Range(0, Height - roomHeight);

            GenerateRoom(new Vector2Int(randomX, randomY), roomWidth, roomHeight, out var startRoom);
            map.StartRoom = startRoom;

            // Generate other rooms
            for (var i = 0; i < MaxRooms; i++)
            {
                for (var j = 0; j < MaxTries; j++)
                {
                    roomWidth = Random.Range(RoomWidth.x, RoomWidth.y);
                    roomHeight = Random.Range(RoomHeight.x, RoomHeight.y);
                    randomX = Random.Range(1, Width - roomWidth);
                    randomY = Random.Range(1, Height - roomHeight);

                    if (GenerateRoom(new Vector2Int(randomX, randomY), roomWidth, roomHeight, out _)) break;
                }
            }

            // Generate corridors
            for (var i = 0; i < rooms.Count; i++)
            {
                GenerateCorridor(rooms[i]);
            }

            // Generate exit
            var exitRoom = rooms[Random.Range(1, rooms.Count)];
            var exitPosition = new Vector2Int(Random.Range(exitRoom.BottomLeft.x, exitRoom.TopRight.x), Random.Range(exitRoom.BottomLeft.y, exitRoom.TopRight.y));
            map.SetValue(exitPosition.x, exitPosition.y, Map.Exit);
            map.ExitRoom = exitRoom;

            // Fix if there are rooms you cant get to
            var canReach = new List<Room>();
            var cantReach = new List<Room>();
            for (var i = 0; i < rooms.Count; i++)
            {
                if (CanReachStartAndExit(rooms[i]))
                {
                    canReach.Add(rooms[i]);
                }
                else
                {
                    cantReach.Add(rooms[i]);
                }
            }

            while (cantReach.Count != 0)
            {
                var room = map.StartRoom.GetClosestRoom(cantReach);
                if (room == null) break;

                while (!CanReachStartAndExit(room))
                {
                    var closestRoom = room.GetClosestRoom(canReach);
                    if (closestRoom == null) break;

                    GenerateCorridor(room);
                }

                cantReach.Remove(room);
                canReach.Add(room);
            }

            return map;
        }

        /// <summary>
        ///     Generates a room.
        /// </summary>
        /// <param name="bottomLeft">the bottom left position of the room.</param>
        /// <param name="width">the width of the room.</param>
        /// <param name="height">the height of the room.</param>
        /// <param name="room">the created room.</param>
        /// <returns>true if room could be generated.</returns>
        bool GenerateRoom(Vector2Int bottomLeft, int width, int height, out Room room)
        {
            var topRight = bottomLeft + new Vector2Int(width, height);
            room = new Room(bottomLeft, topRight);

            if (room.CheckForOverlap(rooms, WallSize)) return false;

            for (var x = bottomLeft.x; x < topRight.x; x++)
            {
                for (var y = bottomLeft.y; y < topRight.y; y++) map.SetValue(x, y, Map.Empty);
            }

            rooms.Add(room);

            return true;
        }

        /// <summary>
        ///     Checks if a room can be reached from the start and exit room.
        /// </summary>
        /// <param name="room">the room to check.</param>
        /// <returns>true if room can reach both start and exit.</returns>
        bool CanReachStartAndExit(Room room)
        {
            var canReachExit = Pathfinder.FindPath(map, room.Center, map.ExitRoom.Center, out _);
            var canReachStart = Pathfinder.FindPath(map, room.Center, map.StartRoom.Center, out _);
            return canReachExit && canReachStart;
        }

        /// <summary>
        ///     Generates a corridor to the closest room that is not connected to the room.
        /// </summary>
        /// <param name="room">the room to generate a corridor from.</param>
        void GenerateCorridor(Room room)
        {
            var otherRoom = room.GetClosestRoom(rooms);
            room.Connect(otherRoom);
            otherRoom.Connect(room);
            var previousRoomCenter = otherRoom.Center;
            var currentRoomCenter = room.Center;

            if (Random.value < 0.5f)
            {
                CreateHorizontalTunnel(previousRoomCenter.x, currentRoomCenter.x, previousRoomCenter.y);
                CreateVerticalTunnel(previousRoomCenter.y, currentRoomCenter.y, currentRoomCenter.x);
            }
            else
            {
                CreateHorizontalTunnel(previousRoomCenter.x, currentRoomCenter.x, currentRoomCenter.y);
                CreateVerticalTunnel(previousRoomCenter.y, currentRoomCenter.y, previousRoomCenter.x);
            }
        }

        /// <summary>
        ///     Creates a horizontal tunnel.
        /// </summary>
        /// <param name="room1X">the first room's center x value.</param>
        /// <param name="room2X">the second room's center x value.</param>
        /// <param name="y">the y value the tunnel will be created at.</param>
        void CreateHorizontalTunnel(int room1X, int room2X, int y)
        {
            for (var x = Mathf.Min(room1X, room2X); x <= Mathf.Max(room1X, room2X); x++)
            {
                map.SetValue(x, y, Map.Empty);
            }
        }

        /// <summary>
        ///     Creates a vertical tunnel.
        /// </summary>
        /// <param name="room1Y">the first room's center y value.</param>
        /// <param name="room2Y">the second room's center y value.</param>
        /// <param name="x">the x value the tunnel will be created at.</param>
        void CreateVerticalTunnel(int room1Y, int room2Y, int x)
        {
            for (var y = Mathf.Min(room1Y, room2Y); y <= Mathf.Max(room1Y, room2Y); y++)
            {
                map.SetValue(x, y, Map.Empty);
            }
        }
    }
}