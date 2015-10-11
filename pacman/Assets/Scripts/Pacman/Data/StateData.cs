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
	}
}