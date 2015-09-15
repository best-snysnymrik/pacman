using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Logic
{
	public class Levels
	{
		public List<MazeDef> levels;
	}
	
	public class MazeDef
	{
		public string name;
		public MazeViewDef view;
	}
	
	public class MazeViewDef
	{
		public int columns;
		public int rows;
		public List<int> elements;
	}
}