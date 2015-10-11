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
		
		public int Level { get; private set; }
		public int Scores { get; private set; }
		public int Lives { get; private set; }
		
		private int dotCount = 0;
		public int DotCount 
		{ 
			get { return dotCount; } 
			set
			{
				dotCount = value;
				
				EnterEnemies(value);
				DropBonus(value);
			}
		}
		
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
		
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		private Queue<EnemyBehaviorPeriodDef> enemyBehaviorPeriods = new Queue<EnemyBehaviorPeriodDef>();
		private EnemyBehaviorPeriodDef currentEnemyBehaviorPeriod; 

		
		public GameModel()
		{
			Level = gameData.state.mazes[gameController.CurrentMaze].level;
			Scores = gameData.state.mazes[gameController.CurrentMaze].scores;
			Lives = gameData.state.mazes[gameController.CurrentMaze].lives;
			DotCount = gameData.state.mazes[gameController.CurrentMaze].dots;
		}
		
		public void AddUnit(UnitModel unit)
		{
			if (unit.UnitId == UnitDefId.Pacman)
				pacman = unit;
			else
				enemies.Add(unit);
		}
		
		public void StartGame()
		{
			SetObservation();
			
			StartEnemyBehaviorPeriods();
			
			MoveEnteredEnemies(DotCount);
			EnterEnemies(DotCount);
			
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
			frighteningTime = gameData.defs.levels[Level].enemyBehavior.frighteningTime;
			UnitsBehaviorMode = UnitBehaviorMode.frightening;
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
			var units = gameData.state.mazes[gameController.CurrentMaze].units.Keys;
			
			foreach (var unitID in units)
			{
				if (unitDefs[unitID].entryDotCount == currentDotCount)
				{
					var enemy = enemies.Find(x => x.UnitId == unitID);
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
			var units = gameData.state.mazes[gameController.CurrentMaze].units.Keys;
			
			foreach (var unitID in units)
			{
				if (currentDotCount > unitDefs[unitID].entryDotCount ||
					currentDotCount == unitDefs[unitID].entryDotCount && currentDotCount > 0)
				{
					var enemy = enemies.Find(x => x.UnitId == unitID);
					if (enemy != null)
						enemy.Move();
				}
			}
		}
		
		private void DropBonus(int currentDotCount)
		{
		}
		
		private void StartEnemyBehaviorPeriods()
		{
			enemyBehaviorPeriods = new Queue<EnemyBehaviorPeriodDef>(gameData.defs.levels[Level].enemyBehavior.periods);			
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
		
		public void FixedUpdate()
		{
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