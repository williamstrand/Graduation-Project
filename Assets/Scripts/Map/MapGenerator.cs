using System.Collections.Generic;
using UnityEngine;

namespace WSP.Map
{
    public class MapGenerator
    {
        const int MaxTries = 1000;

        public Vector2Int StartRoomWidth { get; set; }
        public Vector2Int StartRoomHeight { get; set; }

        public Vector2Int RoomWidth { get; set; }
        public Vector2Int RoomHeight { get; set; }

        public int WallSize { get; set; } = 1;

        List<Room> rooms = new();
        Grid grid;

        public Grid GenerateMap(int width, int height, float cellSize, int maxRooms)
        {
            grid = new Grid(width, height, cellSize);

            // Fill grid with walls
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++) grid.SetValue(x, y, Grid.Wall);
            }

            // Generate start room
            var roomWidth = Random.Range(StartRoomWidth.x, StartRoomWidth.y);
            var roomHeight = Random.Range(StartRoomHeight.x, StartRoomHeight.y);
            var randomX = Random.Range(0, width - roomWidth);
            var randomY = Random.Range(0, height - roomHeight);
            GenerateRoom(new Vector2Int(randomX, randomY), roomWidth, roomHeight);

            // Generate other rooms
            for (var i = 0; i < maxRooms; i++)
            {
                for (var j = 0; j < MaxTries; j++)
                {
                    roomWidth = Random.Range(RoomWidth.x, RoomWidth.y);
                    roomHeight = Random.Range(RoomHeight.x, RoomHeight.y);
                    randomX = Random.Range(1, width - roomWidth);
                    randomY = Random.Range(1, height - roomHeight);

                    if (GenerateRoom(new Vector2Int(randomX, randomY), roomWidth, roomHeight)) break;
                }
            }

            // Generate corridors
            GenerateCorridors();

            // Generate exit
            var exitRoom = rooms[Random.Range(1, rooms.Count)];
            var exitPosition = new Vector2Int(Random.Range(exitRoom.BottomLeft.x, exitRoom.TopRight.x), Random.Range(exitRoom.BottomLeft.y, exitRoom.TopRight.y));
            grid.SetValue(exitPosition.x, exitPosition.y, Grid.Exit);

            return grid;
        }

        void GenerateCorridors()
        {
            for (var i = 0; i < rooms.Count; i++)
            {
                GenerateCorridor(rooms[i]);
            }

            for (var i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].ConnectedRooms.Count == 0)
                {
                    GenerateCorridor(rooms[i]);
                }
            }
        }

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
                CreateVerticalTunnel(previousRoomCenter.y, currentRoomCenter.y, previousRoomCenter.x);
                CreateHorizontalTunnel(previousRoomCenter.x, currentRoomCenter.x, currentRoomCenter.y);
            }
        }

        void CreateHorizontalTunnel(int x1, int x2, int y)
        {
            for (var x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
            {
                grid.SetValue(x, y, Grid.Empty);
            }
        }

        void CreateVerticalTunnel(int y1, int y2, int x)
        {
            for (var y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            {
                grid.SetValue(x, y, Grid.Empty);
            }
        }

        bool GenerateRoom(Vector2Int bottomLeft, int sizeX, int sizeY)
        {
            var topRight = bottomLeft + new Vector2Int(sizeX, sizeY);
            var room = new Room(bottomLeft, topRight);

            if (room.CheckForOverlap(rooms, WallSize))
            {
                Debug.Log("Failed to generate room");
                return false;
            }

            for (var x = bottomLeft.x; x < topRight.x; x++)
            {
                for (var y = bottomLeft.y; y < topRight.y; y++) grid.SetValue(x, y, Grid.Empty);
            }

            rooms.Add(room);

            return true;
        }


    }
}