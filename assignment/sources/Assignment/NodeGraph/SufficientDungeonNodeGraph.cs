using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

class SufficientDungeonNodeGraph : NodeGraph
{
    private Dungeon _dungeon;

    public SufficientDungeonNodeGraph(Dungeon pDungeon) : base((int)(pDungeon.size.Width * pDungeon.scale),
        (int)(pDungeon.size.Height * pDungeon.scale), (int)pDungeon.scale / 3)
    {
        _dungeon = pDungeon;
    }

    protected override void generate()
    {
        //stores the nodes at the center of each room so they can easily be accessed when adding in the door nodes.
        Dictionary<Room, Node> roomNodes = new Dictionary<Room, Node>() ;
        
        //loop over every room
        foreach (Room room in _dungeon.rooms)
        {
            //get the center of the room
            Node node = new Node(room.getCenter(_dungeon.scale));
            
            //add node to the node list and add it to the roomNodes Dictionary
            nodes.Add(node);
            roomNodes.Add(room,node);
            Console.WriteLine("created room node at " + room.getCenter(_dungeon.scale));
        }

        //loop over every door in order to add in their nodes.
        foreach (Door door in _dungeon.doors)
        {
            //create node based on door position.
            Node node = new Node(new Point(Mathf.Round((door.location.X + .5f) * _dungeon.scale),
                Mathf.Round((door.location.Y + .5f) * _dungeon.scale)));
            
            //add node.
            nodes.Add(node);
            
            //retrieve the room nodes from the dictionary and add connections.
            Node roomA = roomNodes[door.roomA];
            Node roomB = roomNodes[door.roomB];
            AddConnection(node,roomA);
            AddConnection(node,roomB);
            Console.WriteLine("created door node at " + door.location);
            Console.WriteLine("added connection with room node at " + door.roomA.getCenter(_dungeon.scale));
            Console.WriteLine("added connection with room node at " + door.roomB.getCenter(_dungeon.scale));
        }
    }
}