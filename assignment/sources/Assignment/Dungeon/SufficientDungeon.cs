using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

class SufficientDungeon : Dungeon
{
    public SufficientDungeon(Size pSize) : base(pSize)
    {
    }

    protected override void generate(int pMinimumRoomSize)
    {
        //add the initial room
        rooms.Add(new Room(new Rectangle(0, 0, size.Width, size.Height)));

        //keep looping until it cannot find a room to split
        Console.WriteLine("Starting room creation");
        bool foundPossiblePartition = true;
        while (foundPossiblePartition)
        {
            foundPossiblePartition = false;
            List<Room> newRooms = new List<Room>();
            newRooms.AddRange(rooms);
            
            //loop over each exiting room
            foreach (Room room in rooms)
            {
                //check room orientation
                if (room.area.Width > room.area.Height)
                {
                    //check if the selected side is large enough to be split correctly
                    if (room.area.Width / 2 >= pMinimumRoomSize)
                    {
                        //get the random point where the room is split
                        int splitPoint = Utils.Random(room.area.Left + pMinimumRoomSize,
                            room.area.Right - pMinimumRoomSize) - room.area.Left;
                        
                        //create 2 new rooms with the split point being the spot where they meet
                        Room room1 = new Room(new Rectangle(room.area.X, room.area.Y, splitPoint+1,
                            room.area.Height));
                        
                        Console.WriteLine("Created a room of size = " + room1.area.Size);
                        
                        newRooms.Add(room1);
                        newRooms.Add(new Room(new Rectangle(room.area.X + room1.area.Width-1, room.area.Y,
                            room.area.Width-room1.area.Width+1, room.area.Height)));
                        
                        //remove the room that was split
                        newRooms.Remove(room);
                        
                        //turn the variable that is used for the while loop to true
                        foundPossiblePartition = true;
                    }
                }
                else
                {
                    //same as the one above but with a few minor number changes.
                    if (room.area.Height / 2 >= pMinimumRoomSize)
                    {
                        int splitPoint = Utils.Random(room.area.Top + pMinimumRoomSize,
                            room.area.Bottom - pMinimumRoomSize) - room.area.Top;
                        Room room1 = new Room(new Rectangle(room.area.X, room.area.Y, room.area.Width,
                            splitPoint + 1));
                        
                        Console.WriteLine("Created a room of size = " + room1.area.Size);
                        
                        newRooms.Add(room1);
                        newRooms.Add(new Room(new Rectangle(room.area.X,room.area.Y + room1.area.Height-1,room.area.Width,room.area.Height-room1.area.Height+1)));
                        newRooms.Remove(room);
                        foundPossiblePartition = true;
                    }
                }
            }
            
            
            //remove all the old rooms and add the new ones.
            rooms.Clear();
            rooms.AddRange(newRooms);
        }
        Console.WriteLine("Starting door creation");

        //Add doors to the rooms
        //Loop over every room.
        for (int i = 0; i < rooms.Count; i++)
        {
            //loop over every room coming after the one selected in the for statement above this comment.
            for (int j = i+1; j < rooms.Count; j++)
            {
                //get the intersection point between the two rooms
                Room intersectionArea = new Room(rooms[i].area);
                intersectionArea.area.Intersect(rooms[j].area);

                //check the orientation between the two
                if (intersectionArea.area.Width > intersectionArea.area.Height)
                {
                    //its horizontal;
                    //verify that it is in fact thin enough to place a door
                    if (intersectionArea.area.Width>2)
                    {
                        //get the random position for the door
                        int xPos = Utils.Random(intersectionArea.area.Left + 1, intersectionArea.area.Right - 1);
                        
                        //create the door
                        Door door = new Door(new Point(xPos, intersectionArea.area.Y));
                        
                        //add the rooms to the doors, this is usefull when creating the node graph
                        door.roomA = rooms[i];
                        door.roomB = rooms[j];
                        
                        //add the door to the door list
                        doors.Add(door);
                        Console.WriteLine("Creating door in horizontal wall at " + door.location);
                    }
                }
                else
                {
                    //its vertical;
                    //same as above but with changes to account for the different orientation.
                    if (intersectionArea.area.Height>2)
                    {
                        int yPos = Utils.Random(intersectionArea.area.Top + 1, intersectionArea.area.Bottom - 1);
                        Door door = new Door(new Point(intersectionArea.area.X, yPos));
                        door.roomA = rooms[i];
                        door.roomB = rooms[j];
                        doors.Add(door);
                        Console.WriteLine("Creating door in horizontal wall at " + door.location);
                    }
                }
            }
        }
    }
}