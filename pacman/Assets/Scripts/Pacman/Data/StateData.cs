using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Data
{
	public class State
	{
		public Dictionary<string, Maze> mazes;
	}
	
	public class Maze
	{
		public int level;
		public int scores;
		public int dots;
		public int lives;
		public List<int> elements;
		public Dictionary<string, UnitPoint> units;
	}
	
	public class UnitPoint
	{
		public Point position;
		public int direction;
	}
	
	public class Point
	{
		public float x;
		public float y;
		
		public override bool Equals(System.Object obj)
	    {
	        if (obj == null)
	        	return false;
	        
	        Point p = obj as Point;
	        if ((System.Object)p == null)
	        	return false;
	        
	        return (x == p.x) && (y == p.y);
	    }
	
	    public bool Equals(Point p)
	    {
	        if ((object)p == null)
	        	return false;
	        
	        return (x == p.x) && (y == p.y);
	    }
	
	    public override int GetHashCode()
	    {
	        return x.GetHashCode() ^ y.GetHashCode();
	    }
	}
}