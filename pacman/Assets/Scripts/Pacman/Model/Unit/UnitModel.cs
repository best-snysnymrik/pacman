using UnityEngine;

using System;
using System.Collections.Generic;

using Common;

using Pacman.Data;
using Pacman.Model;
using Pacman.View.Units;

namespace Pacman.Model.Unit
{
	public class UnitModel : IObservable<UnitInfo>
	{
		protected GameData gameData = GameData.gameData;
		protected GameController gameController = GameController.gameController;
		
		private UnitBehaviorMode unitBehaviorMode;
		public UnitBehaviorMode UnitBehaviorMode 
		{ 
			get { return unitBehaviorMode; } 
			set
			{
				unitBehaviorMode = value;
				Speed = CalculateSpeed(value);
			}
		}
		
		public string UnitId { get; protected set; }
		public float Speed { get; private set; }
		
		private UnitPosition currentPosition;
		protected UnitPosition CurrentPosition
		{
			get { return currentPosition; }
			set
			{
				currentPosition = value;
				UpdatePosition();
			}
		}
		
		private const int stepLength = 1;
		protected Dictionary<Direction, Vector2> steps = new Dictionary<Direction, Vector2>()
		{
			{Direction.up, new Vector2(-stepLength, 0)},
			{Direction.left, new Vector2(0, -stepLength)},
			{Direction.down, new Vector2(stepLength, 0)},
			{Direction.right, new Vector2(0, stepLength)}
		};		
		
		private Dictionary<Direction, Vector2> portMovements = new Dictionary<Direction, Vector2>();
		
		protected MazeModel maze;
		protected UnitMediator mediator;
		
		private List<IObserver<UnitInfo>> observers = new List<IObserver<UnitInfo>>();
				
		public static UnitModel CreateUnitModel(string unitId)
		{
			switch (unitId)
            {
				case UnitDefId.Pacman:
					return new PacmanModel();
				case UnitDefId.Blinky:
					return new BlinkyModel();
				case UnitDefId.Pinky:
					return new PinkyModel();
				case UnitDefId.Inky:
					return new InkyModel();
				case UnitDefId.Clyde:
					return new ClydeModel();
			}
			
			return null;
		}
		
		public void SetMediator(UnitMediator mediator)
		{
			this.mediator = mediator;
			mediator.SetUnitModel(this);
		}
		
		public void SetMazeModel(MazeModel maze)
		{
			this.maze = maze;			
			InitMazeData();
		}
		
		protected virtual void InitMazeData()
		{
			var currentPoint = maze.Units[UnitId];	
			CurrentPosition = new UnitPosition(new Vector2(currentPoint.position.x, currentPoint.position.y), (Direction)currentPoint.direction);
			
			portMovements[Direction.up] = new Vector2(maze.Rows - 1, 0);
			portMovements[Direction.left] = new Vector2(0, maze.Cols - 1);
			portMovements[Direction.down] = - new Vector2(maze.Rows - 1, 0);
			portMovements[Direction.right] = - new Vector2(0, maze.Cols - 1);
		}
		
		protected virtual void Init()
		{
			Speed = CalculateSpeed(UnitBehaviorMode);	
		}
		
		public void Move()
		{
			MoveUnit();
		}
		
		public virtual void StartMove()
		{
			MoveUnit();
		}
		
		private void MoveUnit()
		{
			try
			{
				mediator.Move();
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}			
		}
		
		public virtual UnitPosition GetNextMovePoint()
		{
			return CurrentPosition;
		}
		
		/// <summary>
		/// проверка, стоит ли юнит в точке края лабиринта,
		/// из которой он перенесется на другой край лабиринта
		/// </summary>
		protected void CheckIsPortMovement()
		{
			if (!maze.IsPointOfType(CurrentPosition.point, MazeElementDefId.port))
				return;
			
			// переносим юнит на другой край лабиринта
			Vector2 newPoint = CurrentPosition.point + portMovements[CurrentPosition.direction];
			
			CurrentPosition.point = newPoint;
			UpdatePosition();
			
			SetUnitToPoint(newPoint);
		}
		
		private void SetUnitToPoint(Vector2 point)
		{
			try
			{
				mediator.SetToPoint(point);
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}
		}
		
		public virtual void Catch()
		{
			SetUnitToStartPoint();
		}
		
		private void SetUnitToStartPoint()
		{
			Stop();
			
			var startPoint = gameData.defs.mazes[gameController.CurrentMaze].units[UnitId].position;
			var startPosition = new Vector2(startPoint.x, startPoint.y);
			
			var startDirection = gameData.defs.mazes[gameController.CurrentMaze].units[UnitId].direction;
			
			CurrentPosition = new UnitPosition(startPosition, (Direction)startDirection);
			
			SetUnitToPoint(startPosition);
			
			StartMove();
		}
		
		private void Stop()
		{
			try
			{
				mediator.Stop();
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}
		}
		
		private float CalculateSpeed(UnitBehaviorMode mode)
		{
			var level = gameData.state.mazes[gameController.CurrentMaze].level;
			
			string unitType = UnitType.Enemy;			
			if (UnitId == UnitDefId.Pacman)
				unitType = UnitType.Pacman;
			
			float baseSpeed = gameData.defs.units[UnitId].baseSpeed;
			float modeFactor = gameData.defs.levels[level].speed[unitType].unitBehaviorFactors[mode.ToString()];
			
			return baseSpeed * modeFactor;
		}
		
		#region IObservable implementation
		public IDisposable Subscribe(IObserver<UnitInfo> observer)
		{
			if (!observers.Contains(observer))
			{
				observers.Add(observer);
				observer.OnNext(new UnitInfo(UnitId, CurrentPosition));
			}
				
			return new Unsubscriber<UnitInfo>(observers, observer);
		}
		
		protected void UpdatePosition()
		{
			Point newPoint = new Point();
			newPoint.x = CurrentPosition.point.x;
			newPoint.y = CurrentPosition.point.y;
			
			gameData.state.mazes[gameController.CurrentMaze].units[UnitId].position = newPoint;
			gameData.state.mazes[gameController.CurrentMaze].units[UnitId].direction = (int)CurrentPosition.direction;
			
			foreach (var observer in observers)
                  observer.OnNext(new UnitInfo(UnitId, CurrentPosition));
		}
		#endregion
	}
	
	
	public class UnitPosition
	{
		public Vector2 point;
		public Direction direction;
		
		public UnitPosition(Vector2 point, Direction direction)
		{
			this.point = point;
			this.direction = direction;
		}
	}
	
	public struct UnitInfo
	{
		public string id;
		public UnitPosition position;
		
		public UnitInfo(string id, UnitPosition position)
		{
			this.id = id;
			this.position = position;
		}
	}
}