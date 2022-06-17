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
        Dictionary<Room, Node> roomNodes = new Dictionary<Room, Node>() ;
        foreach (Room room in _dungeon.rooms)
        {
            Node node = new Node(room.getCenter(_dungeon.scale));
            nodes.Add(node);
            roomNodes.Add(room,node);
        }

        foreach (Door door in _dungeon.doors)
        {
            Node node = new Node(new Point(Mathf.Round((door.location.X + .5f) * _dungeon.scale),
                Mathf.Round((door.location.Y + .5f) * _dungeon.scale)));
            nodes.Add(node);

            Console.WriteLine((door.roomA == null) + " : " + (door.roomB == null));
            
            Node roomA = roomNodes[door.roomA];
            Node roomB = roomNodes[door.roomB];
            AddConnection(node,roomA);
            AddConnection(node,roomB);
        }
    }
}