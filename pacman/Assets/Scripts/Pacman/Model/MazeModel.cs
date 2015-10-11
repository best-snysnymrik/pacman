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
		
		public bool IsPortPoint(Vector2 point)
		{
			int index = (int)(point.x * Cols + point.y);
			
			if (Elements[index] == (int)MazeElementDefId.port)
				return true;
			
			return false;
		}
	}
}