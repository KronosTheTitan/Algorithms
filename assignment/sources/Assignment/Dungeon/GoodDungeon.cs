﻿using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

class GoodDungeon : Dungeon
{
    public GoodDungeon(Size pSize) : base(pSize)
    {
    }

    protected override void generate(int pMinimumRoomSize)
    {
        rooms.Add(new Room(new Rectangle(0, 0, size.Width, size.Height)));
        
        Dictionary<Room, int> doorCount = new Dictionary<Room, int>();

        bool foundPossiblePartion = true;
        while (foundPossiblePartion)
        {
            foundPossiblePartion = false;
            List<Room> newRooms = new List<Room>();
            newRooms.AddRange(rooms);

            foreach (Room room in rooms)
            {
                if (room.area.Width > room.area.Height)
                {
                    if (room.area.Width / 2 >= pMinimumRoomSize)
                    {
                        int splitPoint = Utils.Random(room.area.Left + pMinimumRoomSize,
                            room.area.Right - pMinimumRoomSize) - room.area.Left;
                        Room room1 = new Room(new Rectangle(room.area.X, room.area.Y, splitPoint+1,
                            room.area.Height));
                        newRooms.Add(room1);
                        newRooms.Add(new Room(new Rectangle(room.area.X + room1.area.Width-1, room.area.Y,
                            room.area.Width-room1.area.Width+1, room.area.Height)));
                        newRooms.Remove(room);
                        foundPossiblePartion = true;
                    }
                }
                else
                {
                    if (room.area.Height / 2 >= pMinimumRoomSize)
                    {
                        int splitPoint = Utils.Random(room.area.Top + pMinimumRoomSize,
                            room.area.Bottom - pMinimumRoomSize) - room.area.Top;
                        Room room1 = new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width,
                            splitPoint + 1));
                        newRooms.Add(room1);
                        newRooms.Add(new Room(new Rectangle(room.area.X,room.area.Y + room1.area.Height-1,room.area.Width,room.area.Height-room1.area.Height+1)));
                        newRooms.Remove(room);
                        foundPossiblePartion = true;
                    }
                }
            }

            rooms.Clear();
            rooms.AddRange(newRooms);
        }

        int smallest = int.MaxValue;
        int largest = int.MinValue;

        List<Room> removal = new List<Room>();

        foreach (Room room in rooms)
        {
            if (room.area.Size.Width*room.area.Size.Height > largest)
            {
                largest = room.area.Size.Width*room.area.Size.Height;
                continue;
            }
            if (room.area.Size.Width*room.area.Size.Height < smallest)
            {
                smallest = room.area.Size.Width*room.area.Size.Height;
                continue;
            }
        }

        foreach (Room room in rooms)
        {
            if(room.area.Size.Width*room.area.Size.Height == smallest || room.area.Size.Width*room.area.Size.Height == largest ) removal.Add(room);
            else
            {
                doorCount.Add(room,0);
            }
        }

        foreach (Room room in removal)
        {
            rooms.Remove(room);
        }

        //Add in doors

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i+1; j < rooms.Count; j++)
            {
                Room intersectionArea = new Room(rooms[i].area);
                intersectionArea.area.Intersect(rooms[j].area);

                if (intersectionArea.area.Width > intersectionArea.area.Height)
                {
                    //its horizontal;
                    if (intersectionArea.area.Width>2)
                    {
                        int xPos = Utils.Random(intersectionArea.area.Left + 1, intersectionArea.area.Right - 1);
                        Door door = new Door(new Point(xPos, intersectionArea.area.Y));
                        door.roomA = rooms[i];
                        door.roomB = rooms[j];
                        doors.Add(door);
                        doorCount[rooms[i]] += 1;
                        doorCount[rooms[j]] += 1;
                    }
                }
                else
                {
                    //its vertical;
                    if (intersectionArea.area.Height>2)
                    {
                        int yPos = Utils.Random(intersectionArea.area.Top + 1, intersectionArea.area.Bottom - 1);
                        Door door = new Door(new Point(intersectionArea.area.X, yPos));
                        door.roomA = rooms[i];
                        door.roomB = rooms[j];
                        doors.Add(door);
                        doorCount[rooms[i]] += 1;
                        doorCount[rooms[j]] += 1;
                    }
                }
            }
        }

        foreach (Room room in rooms)
        {
            if (doorCount[room] == 0)
            {
                room.color = Brushes.Red;
            }

            if (doorCount[room] == 1)
            {
                room.color = Brushes.Orange;
            }

            if (doorCount[room] == 2)
            {
                room.color = Brushes.Yellow;
            }

            if (doorCount[room] >= 3)
            {
                room.color = Brushes.Green;
            }
        }
    }

    protected override void drawRooms(IEnumerable<Room> pRooms, Pen pWallColor, Brush pFillColor = null)
    {
        foreach (Room room in pRooms)
        {
            drawRoom(room, pWallColor, room.color);
        }
    }
}