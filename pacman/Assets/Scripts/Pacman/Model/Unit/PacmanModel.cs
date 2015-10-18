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
			nextPosition.direction = direction;
		}
		
		public override UnitPosition GetNextMovePoint()
		{
			CurrentPosition = nextPosition;
			
			CollectDot();
			CollectBonus();
			
			CheckIsPortMovement();
			
			var point = CurrentPosition.point;
			var direction = CurrentPosition.direction;
			
			if (maze.StepIsPossible(point, direction))
			{
				nextPosition = new UnitPosition(point + steps[direction], direction);
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
			base.Catch();
			gameController.PacmanCatched();
		}
	}
}