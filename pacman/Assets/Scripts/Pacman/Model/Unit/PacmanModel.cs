using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Model.Unit
{
	public class PacmanModel : UnitModel
	{
		public delegate void PacmanInBonusPointHandler();
		public event PacmanInBonusPointHandler PacmanInBonusPoint;
		
		private List<MazeElementDefId> dotTypes = new List<MazeElementDefId>()
		{
			MazeElementDefId.dot,
			MazeElementDefId.energizer
		};
		
		public PacmanModel()
		{
			UnitId = UnitDefId.Pacman;
			
			Init();
		}
		
		protected override void Init()
		{
			base.Init();
		}
		
		public void SetMoveDirection(Direction direction)
		{
			CurrentPosition.direction = direction;
			UpdatePosition();
		}
		
		public override UnitPosition GetNextMovePoint()
		{
			CollectDot();
			CollectBonus();
			
			CheckIsPortMovement();
			
			var point = CurrentPosition.point;
			var direction = CurrentPosition.direction;
			
			if (maze.StepIsPossible(point, direction))
			{
				var nextPosition = new UnitPosition(point + steps[direction], direction);				
				CurrentPosition = nextPosition;
				
				return nextPosition;
			}
			
			return CurrentPosition;
		}
		
		protected void CollectDot()
		{
			foreach (var dotType in dotTypes)
				maze.CollectDot(CurrentPosition.point, dotType);
		}
		
		private void CollectBonus()
		{
			if (maze.IsBonusPoint(CurrentPosition.point))
				PacmanInBonusPoint();
		}
		
		public override void Catch()
		{
			gameController.PacmanCatched();
			base.Catch();
		}
	}
}