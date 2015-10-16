using UnityEngine;
using System.Collections.Generic;

using Pacman.Data;
using Pacman.Model.Unit;

namespace Pacman.Model
{
	public enum MazeElementDefId
	{
		empty,
		floor,
		dot,
		energizer,
		port,
		door,		
		wall
	}
	
	public class MazeModel
	{
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		public delegate void DotCollectedHandler(Vector2 point);
		public event DotCollectedHandler OnDotCollected;
		
		public delegate void DotCountChangedHandler(int count);
		public event DotCountChangedHandler OnDotCountChanged;
		
		public delegate void EnergizerCollectedHandler();
		public event EnergizerCollectedHandler OnEnergizerCollected;
		
		private int allDotCount;
		private int dotCount = 0;
		public int DotCount 
		{ 
			get { return dotCount; } 
			set
			{
				dotCount = value;
				gameData.state.mazes[gameController.CurrentMaze].dots = dotCount;
				
				if (OnDotCountChanged != null)
					OnDotCountChanged(value);
				
				if (dotCount == allDotCount)
					Debug.Log("win");
			}
		}
		
		public List<int> Elements { get; private set; }
		public int Cols { get; private set; }
		public int Rows { get; private set; }
		
		public Vector2 EnemyStartPosition { get; private set; }
		public Vector2 BonusPosition { get; private set; }
		
		public Dictionary<string, UnitPoint> Units { get; private set; }
				
		public MazeModel()
		{
			Elements = gameData.state.mazes[gameController.CurrentMaze].elements;
			
			Cols = gameData.defs.mazes[gameController.CurrentMaze].view.columns;
			Rows = gameData.defs.mazes[gameController.CurrentMaze].view.rows;
			
			var enemyStartPoint = gameData.defs.mazes[gameController.CurrentMaze].view.enemyStartPoint;
			EnemyStartPosition = new Vector2(enemyStartPoint.x, enemyStartPoint.y);
			
			var bonusPoint = gameData.defs.mazes[gameController.CurrentMaze].view.bonusPoint;
			BonusPosition = new Vector2(bonusPoint.x, bonusPoint.y);
			
			Units = gameData.state.mazes[gameController.CurrentMaze].units;
			
			allDotCount = gameData.defs.mazes[gameController.CurrentMaze].view.dotCount;
			DotCount = gameData.state.mazes[gameController.CurrentMaze].dots;			
		}
		
		public bool StepIsPossible(Vector2 point, Direction stepDirection)
		{
			int index = (int)(point.x * Cols + point.y);
			
			switch (stepDirection)
			{
				case Direction.left:
					return index % Cols != 0 && Elements[index - 1] < (int)MazeElementDefId.door;
				case Direction.right:
					return (index + 1) % Cols != 0 && Elements[index + 1] < (int)MazeElementDefId.door;
				case Direction.up:
					return (index - Cols) >= 0 && Elements[index - Cols] < (int)MazeElementDefId.door;
				case Direction.down:
					return (index + Cols) <= (Cols * Rows) && Elements[index + Cols] < (int)MazeElementDefId.door;
			}
			
			return false;
		}
				
		public bool IsPointOfType(Vector2 point, MazeElementDefId elementDefId)
		{
			int index = (int)(point.x * Cols + point.y);
			
			if (Elements[index] == (int)elementDefId)
				return true;
			
			return false;
		}
				
		public void CollectDot(Vector2 point, MazeElementDefId dotType)
		{
			if (!IsPointOfType(point, dotType))
				return;
			
			DotCount += 1;
			
			gameController.CollectDot(dotType);
			
			int index = (int)(point.x * Cols + point.y);
			Elements[index] = (int)MazeElementDefId.floor;
			
			if (OnDotCollected != null)
				OnDotCollected(point);
			
			if (dotType == MazeElementDefId.energizer && OnEnergizerCollected != null)
				OnEnergizerCollected();	
		}
		
		public bool IsBonusPoint(Vector2 point)
		{
			var bonusPoint = gameData.defs.mazes[gameController.CurrentMaze].view.bonusPoint;
			return point.Equals(new Vector2(bonusPoint.x, bonusPoint.y));
		}
	}
}