using UnityEngine;
using System.Collections;

namespace Pacman.Model
{
	public static class UnitDefId
	{
		public const string Pacman = "Pacman";
		public const string Blinky = "Blinky";
		public const string Pinky = "Pinky";
		public const string Inky = "Inky";
		public const string Clyde = "Clyde";
	}
	
	public static class UnitType
	{
		public const string Pacman = "Pacman";
		public const string Enemy = "Enemy";
	}
	
	public enum EnemyBehaviorMode
	{
		chase,
		scatter
	}
	
	public enum UnitBehaviorMode
	{
		normal,
		frightening
	}
	
	public enum Direction
	{
		right,
		down,
		left,
		up
	}
}