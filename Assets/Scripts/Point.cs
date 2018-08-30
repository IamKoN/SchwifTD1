using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct used for all nodes in the game
public struct Point
{
	public int X { get; set; }
	public int Y { get; set; }
	
	public static bool operator ==(Point first, Point second)
    {
		return first.X == second.X && first.Y == second.Y; 
	}
	public static bool operator !=(Point first, Point second)
    {
		return first.X != second.X || first.Y != second.Y; 
	}
    
	public static Point operator -(Point x, Point y)
    {
		return new Point (x.X - y.Y, x.Y - y.Y); 
	}
    public static Point operator /(Point x, int value)
    {
        return new Point(x.X / value, x.Y / value);
    }

    public Point(int x, int y)
    {
		this.X = x; 
		this.Y = y; 
	}

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        Point other = (Point)obj;
        return X == other.X && Y == other.Y;
    }
}
