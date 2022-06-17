using System;
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;

class ExcelentDungeonNodeGraph : NodeGraph
{
    private Dungeon _dungeon;

    public ExcelentDungeonNodeGraph(Dungeon pDungeon) : base((int)(pDungeon.size.Width * pDungeon.scale),
        (int)(pDungeon.size.Height * pDungeon.scale), (int)pDungeon.scale / 3)
    {
        _dungeon = pDungeon;
    }

    protected override void generate()
    {
        Bitmap bitmap = new Bitmap(_dungeon.size.Width, _dungeon.size.Height);

        foreach (Room room in _dungeon.rooms)
        {
            for (int X = room.area.Left + 1; X < room.area.Right - 1; X++)
            {
                for (int Y = room.area.Top + 1; Y < room.area.Bottom - 1; Y++)
                {
                    bitmap.SetPixel(X, Y, Color.White);
                }
            }
        }

        foreach (Door door in _dungeon.doors)
        {
            bitmap.SetPixel(door.location.X, door.location.Y, Color.White);
        }

        Node[,] nodes2 = new Node[bitmap.Width,bitmap.Height];

        for (int X = 0; X < bitmap.Width; X++)
        {
            for (int Y = 0; Y < bitmap.Height; Y++)
            {
                Console.WriteLine(bitmap.GetPixel(X,Y));
                if (bitmap.GetPixel(X, Y).A != 255) continue;
                Console.WriteLine("hit!");
                
                
                
                Node node = new Node(new Point(Mathf.Round((X + .5f) * _dungeon.scale),
                    Mathf.Round((Y + .5f) * _dungeon.scale)));
                nodes.Add(node);
                nodes2[X, Y] = node;
            }
        }

        for (int X = 0; X < bitmap.Width; X++)
        {
            for (int Y = 0; Y < bitmap.Height; Y++)
            {
                if(nodes2[X,Y] == null) continue;
                if (X+1<bitmap.Width&&nodes2[X + 1, Y] != null)
                {
                    AddConnection(nodes2[X,Y],nodes2[X + 1, Y]);
                }
                if (Y+1<bitmap.Height&&nodes2[X, Y+1] != null)
                {
                    AddConnection(nodes2[X,Y],nodes2[X, Y + 1]);
                }
                if ((Y+1<bitmap.Height&&X+1<bitmap.Width)&&nodes2[X+1, Y+1] != null)
                {
                    AddConnection(nodes2[X,Y],nodes2[X+1, Y + 1]);
                }
                if ((Y-1>=0&&X+1<bitmap.Width)&&nodes2[X+1, Y-1] != null)
                {
                    AddConnection(nodes2[X,Y],nodes2[X+1, Y - 1]);
                }
            }
        }
        bitmap.Save("D:/map.png");
    }
}