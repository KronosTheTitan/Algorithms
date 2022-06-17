using System.Drawing;
using GXPEngine;

/**
 * This class represents (the data for) a Room, at this moment only a rectangle in the dungeon.
 */
class Room
{
	public Rectangle area;

	public Room (Rectangle pArea)
	{
		area = pArea;
	}

	public Point getCenter(float scale)
	{
		return new Point(Mathf.Round(((area.Left+area.Right)/2) *scale),Mathf.Round(((area.Top+area.Bottom)/2)*scale));
	}

	//TODO: Implement a toString method for debugging?
	//Return information about the type of object and it's data
	//eg Room: (x, y, width, height)

}
