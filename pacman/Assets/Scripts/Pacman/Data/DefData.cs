using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Data
{
	public class Defs
	{
		public Dictionary<string, MazeDef> mazes;
		public Dictionary<string, UnitDef> units;
		public Dictionary<string, MazeElementDef> mazeElements;
		public BonusesDef bonuses;
		public CommonValues commonValues;
		public List<LevelDef> levels;
	}
	
	public class MazeDef
	{
		public string name;
		public MazeViewDef view;
		public Dictionary<string, UnitPositionDef> units;
	}
	
	public class UnitDef
	{
		public string prefab;
		public string material;
		public int entryDotCount;
		public float baseSpeed;
	}
	
	public class UnitPositionDef
	{
		public Point position;
		public Point scatterPoint;
		public int direction;		
	}
	
	public class MazeViewDef
	{
		public int columns;
		public int rows;
		public List<int> elements;
		public Point enemyStartPoint;
		public Point bonusPoint;
	}
	
	public class MazeElementDef
	{
		public string prefab;
	}
	
	public class BonusesDef
	{
		public Dictionary<string, BonusTypeDef> types;
		public List<BonusDropMomentDef> dropMoments;
		public int accessTime;
	}
	
	public class BonusTypeDef
	{
		public string prefab;
	}
	
	public class BonusDropMomentDef
	{
		public int dotCount;
	}
	
	public class LevelDef
	{
		public EnemyBehaviorDef enemyBehavior;
		public Dictionary<string, UnitSpeedDef> speed;
		public BonusDef bonus;
	}
	
	public class EnemyBehaviorDef
	{
		public List<EnemyBehaviorPeriodDef> periods;
		public float frighteningTime;
	}
	
	public class EnemyBehaviorPeriodDef
	{
		public int mode;
		public float time;
	}
	
	public class UnitSpeedDef
	{
		public Dictionary<string, float> unitBehaviorFactors;
	}
	
	public class BonusDef
	{
		public string type;
		public int scores;
	}
	
	public class CommonValues
	{
		public int livesCount;
	}
}