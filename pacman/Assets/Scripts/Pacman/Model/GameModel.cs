using UnityEngine;
using System.Collections.Generic;

using Pacman.Data;
using Pacman.Model;
using Pacman.Model.Unit;

namespace Pacman.Model
{
	public class GameModel
	{
		private UnitModel pacman;
		private List<UnitModel> enemies = new List<UnitModel>();
		
		public delegate void BonusDroppedHandler(string bonusType);
		public event BonusDroppedHandler OnBonusDropped;
		
		public delegate void BonusRemovedHandler();
		public event BonusRemovedHandler OnBonusRemoved;
		
		private UnitBehaviorMode unitsBehaviorMode = UnitBehaviorMode.normal;
		public UnitBehaviorMode UnitsBehaviorMode 
		{ 
			get { return unitsBehaviorMode; } 
			set
			{
				unitsBehaviorMode = value;
				SetUnitsBehaviorMode(value);
			}
		}
		
		private float frighteningTime = 0;
		private int catchedFrightenedEnemyCount = 0;
		
		private float bonusAccessTime = 0;
		private bool isBonusTime = false;
		
		private bool isPause = false;
		
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		private Queue<EnemyBehaviorPeriodDef> enemyBehaviorPeriods = new Queue<EnemyBehaviorPeriodDef>();
		private EnemyBehaviorPeriodDef currentEnemyBehaviorPeriod; 
		
		private MazeModel maze;
		private int dotCount = 0;
		
		public GameModel()
		{}
		
		public void SetMazeModel(MazeModel maze)
		{
			this.maze = maze;			
			
			dotCount = maze.DotCount;
			maze.OnDotCountChanged += DotCountChanged;
			maze.OnEnergizerCollected += SetFrighteningBehaviorMode;
		}
		
		public void OnDestroy()
		{
			maze.OnDotCountChanged -= DotCountChanged;
			maze.OnEnergizerCollected -= SetFrighteningBehaviorMode;
			
			foreach (var enemy in enemies)
				((EnemyModel)enemy).OnEnemyCatched -= EnemyCatched;
			
			((PacmanModel)pacman).PacmanInBonusPoint += CollectBonus;
		}
		
		private void DotCountChanged(int count)
		{
			dotCount = count;
			
			EnterEnemies(dotCount);
			DropBonus(dotCount);
		}
		
		public void AddUnit(UnitModel unit)
		{
			if (unit.UnitId == UnitDefId.Pacman)
			{
				pacman = unit;
				((PacmanModel)unit).PacmanInBonusPoint += CollectBonus;
			}
			else
			{
				enemies.Add(unit);
				((EnemyModel)unit).OnEnemyCatched += EnemyCatched;
			}
		}
		
		public void StartGame()
		{
			SetObservation();
			
			StartEnemyBehaviorPeriods();
			
			MoveEnteredEnemies(dotCount);
			EnterEnemies(dotCount);
			
			pacman.StartMove();
		}
		
		public void SetObservation()
		{
			//все привидения следят за пакменом
			foreach (EnemyModel enemy in enemies)
				enemy.Subscribe(pacman);
			
			//инки следит за блинки
			var inky = enemies.Find(x => x.UnitId == UnitDefId.Inky) as EnemyModel;
			var blinky = enemies.Find(x => x.UnitId == UnitDefId.Blinky);
			
			if (inky != null && blinky != null)
				inky.Subscribe(blinky);	
		}
		
		public void SetFrighteningBehaviorMode()
		{
			catchedFrightenedEnemyCount = 0;
			
			frighteningTime = gameData.defs.levels[gameController.Level].enemyBehavior.frighteningTime;
			UnitsBehaviorMode = UnitBehaviorMode.frightening;
			
			foreach (EnemyModel enemy in enemies)
				enemy.SetFrighteningBehavior(frighteningTime);
		}
		
		private void SetUnitsBehaviorMode(UnitBehaviorMode mode)
		{
			foreach (UnitModel enemy in enemies)
				enemy.UnitBehaviorMode = mode;
			
			pacman.UnitBehaviorMode = mode;
		}
		
		private void SetEnemyBehaviorMode(EnemyBehaviorMode mode)
		{
			foreach (EnemyModel enemy in enemies)
				enemy.EnemyBehaviorMode = mode;
		}
		
		/// <summary>
		/// Выпускаем привидений, если ограничивающее их выход
		/// количество собранной еды, съедено 
		/// </summary>
		private void EnterEnemies(int currentDotCount)
		{
			var unitDefs = gameData.defs.units;
			var units = gameData.state.mazes[gameController.CurrentMaze].units;
			
			foreach (var unit in units)
			{
				if (unitDefs[unit.Key].entryDotCount == currentDotCount &&
					unit.Value.position.Equals(gameData.defs.mazes[gameController.CurrentMaze].units[unit.Key].position))
				{
					var enemy = enemies.Find(x => x.UnitId == unit.Key);
					if (enemy != null)
						enemy.StartMove();
				}
			}
		}
		
		/// <summary>
		/// Запускаем привидений, которые уже были выпущены
		/// </summary>
		private void MoveEnteredEnemies(int currentDotCount)
		{
			var unitDefs = gameData.defs.units;
			var units = gameData.state.mazes[gameController.CurrentMaze].units;
			
			foreach (var unit in units)
			{
				if (unit.Key == UnitDefId.Blinky || currentDotCount >= unitDefs[unit.Key].entryDotCount &&
					!unit.Value.position.Equals(gameData.defs.mazes[gameController.CurrentMaze].units[unit.Key].position))
				{
					var enemy = enemies.Find(x => x.UnitId == unit.Key);
					if (enemy != null)
						enemy.Move();
				}
			}
		}
		
		private void DropBonus(int currentDotCount)
		{
			var dropMoments = gameData.defs.bonuses.dropMoments;
			
			foreach (var dropMoment in dropMoments)
			{
				if (currentDotCount == dropMoment.dotCount)
				{
					bonusAccessTime = gameData.defs.bonuses.accessTime;
					
					if (!isBonusTime)
						OnBonusDropped(gameData.defs.levels[gameController.Level].bonus.type);
					
					isBonusTime = true;
					
					return;
				}
			}
		}
		
		private void CollectBonus()
		{
			if (!isBonusTime)
				return;
			
			isBonusTime = false;
			OnBonusRemoved();
			
			gameController.CollectBonus();
		}
		
		private void EnemyCatched()
		{
			gameController.EnemyCatched(catchedFrightenedEnemyCount);
			catchedFrightenedEnemyCount++;
		}
		
		private void StartEnemyBehaviorPeriods()
		{
			enemyBehaviorPeriods = new Queue<EnemyBehaviorPeriodDef>(gameData.defs.levels[gameController.Level].enemyBehavior.periods);			
			ApplyNextEnemyBehaviorMode();
		}
		
		private void ApplyNextEnemyBehaviorMode()
		{
			if (enemyBehaviorPeriods.Count == 0)
				currentEnemyBehaviorPeriod = null;
			else
			{
				currentEnemyBehaviorPeriod = enemyBehaviorPeriods.Dequeue();							
				SetEnemyBehaviorMode((EnemyBehaviorMode)currentEnemyBehaviorPeriod.mode);
			}
		}
		
		public void Pause()
		{
			isPause = true;
			
			iTween.Pause();
		}
		
		public void Resume()
		{
			isPause = false;
			
			iTween.Resume();
		}
		
		public void FixedUpdate()
		{
			if (isPause)
				return;
			
			if (isBonusTime)
			{
				bonusAccessTime -= Time.deltaTime;
				
				if (bonusAccessTime <= 0)
				{
					isBonusTime = false;
					OnBonusRemoved();
				}
			}
				
			if (UnitsBehaviorMode == UnitBehaviorMode.frightening)
			{
				frighteningTime -= Time.deltaTime;
				
				if (frighteningTime <= 0)
					UnitsBehaviorMode = UnitBehaviorMode.normal;
				else
					return;
			}
			
			if (currentEnemyBehaviorPeriod == null)
				return;
			
			currentEnemyBehaviorPeriod.time -= Time.deltaTime;
			
			if (currentEnemyBehaviorPeriod.time <= 0)
				ApplyNextEnemyBehaviorMode();
		}		
	}
}