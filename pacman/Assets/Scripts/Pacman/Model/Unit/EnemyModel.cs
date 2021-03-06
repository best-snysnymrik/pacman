﻿using UnityEngine;
using System;
using System.Collections.Generic;

using Pacman.View.Units;
using Common;

namespace Pacman.Model.Unit
{
	public class EnemyModel : UnitModel, IObserver<UnitInfo>
	{
		protected Vector2 scatterPosition;		
		protected UnitPosition pacmanPosition;
		
		public delegate void EnemyCatchedHandler();
		public event EnemyCatchedHandler OnEnemyCatched;
		
		private EnemyBehaviorMode enemyBehaviorMode = EnemyBehaviorMode.chase;
		public EnemyBehaviorMode EnemyBehaviorMode 
		{ 
			get { return enemyBehaviorMode; } 
			set
			{
				if (enemyBehaviorMode != value)
				{
					// при смене режима преследование/разбегание
					// привидение меняет направление движения на противоположное
					nextPosition.direction = oppositeDirections[nextPosition.direction];
				}
				
				enemyBehaviorMode = value;
			}
		}
		
		private Dictionary<Direction, Direction> oppositeDirections = new Dictionary<Direction, Direction>()
		{
			{Direction.left, Direction.right},
			{Direction.right, Direction.left},
			{Direction.up, Direction.down},
			{Direction.down, Direction.up}
		};
		
		private IDisposable unsubscriber;
		
		
		protected override void Init()
		{
			base.Init();
		}
		
		protected override void InitMazeData()
		{
			base.InitMazeData();
			
			var scatterPoint = gameData.defs.mazes[gameController.CurrentMaze].units[UnitId].scatterPoint;
			scatterPosition = new Vector2(scatterPoint.x, scatterPoint.y);
		}
		
		public void SetFrighteningBehavior(float time)
		{
			try
			{
				((EnemyMediator)mediator).SetFrighteningBehavior(time);
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}
		}

		#region IObserver implementation
		public virtual void Subscribe(IObservable<UnitInfo> provider)
		{
			unsubscriber = provider.Subscribe(this);
		}
		
		public virtual void Unsubscribe()
		{
			unsubscriber.Dispose();
		}
		
		public virtual void OnCompleted() {}
		
        public virtual void OnError(Exception exception) {}
		
        public virtual void OnNext(UnitInfo info)
		{
			pacmanPosition = info.position;
		}
		#endregion
		
		public override void StartMove()
		{
			// TODO: надо придумать решение выхода посимпатичнее
			
			// сначала выводим привидение из "дома"
			nextPosition.point = maze.EnemyStartPosition;
			
			MoveEnemyToStartPoint(maze.EnemyStartPosition);
		}
		
		private void MoveEnemyToStartPoint(Vector2 point)
		{
			try
			{
				mediator.MoveToStart(point);
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}
		}
		
		public override UnitPosition GetNextMovePoint()
		{
			CurrentPosition = nextPosition;
			
			CheckIsPortMovement();
			
			switch (UnitBehaviorMode)
			{
				case UnitBehaviorMode.frightening:
					return GetRandomMovePoint();
				default:
					return GetNextMovePointToTarget();
			}
		}
		
		private UnitPosition GetRandomMovePoint()
		{
			var point = CurrentPosition.point;
			var direction = CurrentPosition.direction;
			
			// обеспечиваем рандомность направления движения
			List<Direction> directions = new List<Direction>(steps.Keys);
			Shuffle<Direction>(directions);
			
			foreach (var stepDirection in directions)
			{
				var step = steps[stepDirection];
				
				if (maze.StepIsPossible(point, stepDirection) && stepDirection != oppositeDirections[direction])
				{
					nextPosition = new UnitPosition(point + step, stepDirection);					
					return nextPosition;
				}
			}
			
			return CurrentPosition;
		}
		
		private UnitPosition GetNextMovePointToTarget()
		{
			var point = CurrentPosition.point;
			var direction = CurrentPosition.direction;
			
			var targetPoint = GetTargetPoint();
			
			float minDistance = maze.Cols * maze.Rows;
			UnitPosition possibleNextPosition = null;
			
			// из всех возможных шагов выбираем тот, 
			// который ближе к целевой точке
			foreach (var step in steps)
			{
				if (maze.StepIsPossible(point, step.Key) && step.Key != oppositeDirections[direction])
				{
					var distance = Vector2.Distance(point + step.Value, targetPoint);
					
					if (distance < minDistance)
					{
						minDistance = distance;
						possibleNextPosition = new UnitPosition(point + step.Value, step.Key);
					}
				}
			}
			
			if (possibleNextPosition == null)
				return CurrentPosition;
			
			nextPosition = possibleNextPosition;
			return nextPosition;
		}
		
		private Vector2 GetTargetPoint()
		{
			switch (EnemyBehaviorMode)
			{
				case EnemyBehaviorMode.chase:
					return GetChasePoint();
				case EnemyBehaviorMode.scatter:
					return scatterPosition;
				default:
					return CurrentPosition.point;
			}
		}
		
		protected virtual Vector2 GetChasePoint()
		{
			return pacmanPosition.point;
		}
		
		public override void Catch()
		{
			OnEnemyCatched();
			base.Catch();
			
			StopFrightening();
		}
		
		private void StopFrightening()
		{
			UnitBehaviorMode = UnitBehaviorMode.normal;
			
			try
			{
				((EnemyMediator)mediator).StopFrighteningBehavior();
			}
			catch (NullReferenceException e)
			{
				Debug.LogException(e);
			}
		}
		
		private void Shuffle<T>(List<T> list)  
		{  
		    int n = list.Count;
			System.Random random = new System.Random();
			
		    while (n > 1) 
			{  
		        n--;  
		        int k = random.Next(n + 1);  
		        T value = list[k];  
		        list[k] = list[n];  
		        list[n] = value;  
		    }  
		}
	}
}