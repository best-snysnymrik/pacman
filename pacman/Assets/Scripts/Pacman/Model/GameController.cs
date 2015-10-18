using UnityEngine;
using System.Collections;

using Pacman.Data;
using Pacman.View.Scene;

namespace Pacman.Model
{
	public enum GameCondition
	{
		MainMenu,
		Game
	}
	
	public class GameController
	{
		private static readonly GameController instance = new GameController();		
		
		static GameController() {}
		private GameController() {}
		
		public static GameController gameController
		{
			get { return instance; }
		}
		
		private SceneView view;
		
		public delegate void GameActivityStateChangedHandler();
		public event GameActivityStateChangedHandler OnPause;
		public event GameActivityStateChangedHandler OnResume;
		public event GameActivityStateChangedHandler OnGameEnd;
		public event GameActivityStateChangedHandler OnNextLevel;
		public event GameActivityStateChangedHandler OnReplay;
		
		public delegate void ValueChangedHandler(int newValue);
		public event ValueChangedHandler OnScoresChanged;
		public event ValueChangedHandler OnLivesChanged;
		
		private GameData gameData = GameData.gameData;
		private GuiManager guiManager = GuiManager.guiManager;

		private GameCondition condition;		
		public GameCondition Condition 
		{
			get { return condition;}
			private set 
			{
				condition = value;
				ChangeScene(condition);					
			}
		}
		
		private string currentMaze;
		public string CurrentMaze
		{
			get { return currentMaze; } 
			set
			{
				currentMaze = value;
			}
		}
		
		private int scores = 0;
		public int Scores
		{ 
			get { return scores; } 
			private set
			{
				scores = value;
				gameData.state.mazes[CurrentMaze].scores = scores;
				
				if (OnScoresChanged != null)
					OnScoresChanged(scores);
			}
		}
		
		private int level = 0;
		public int Level
		{ 
			get { return level; }
			private set
			{
				level = value;
				gameData.state.mazes[CurrentMaze].level = level;
			}
		}
		
		private int lives = 0;
		public int Lives
		{
			get { return lives; }
			private set
			{
				lives = value;
				gameData.state.mazes[CurrentMaze].lives = lives;
				
				if (OnLivesChanged != null)
					OnLivesChanged(lives);
				
				if (lives == 0)
					GameOver();
			}
		}
		
		
		public void SetSceneView(SceneView view)
		{
			this.view = view;
		}
		
		private void ChangeScene(GameCondition condition)
		{
			view.ClearScene();
			view.ShowScene(condition);
		}
		
		private void SetMazeProgress()
		{
			Scores = gameData.state.mazes[CurrentMaze].scores;
			Level = gameData.state.mazes[CurrentMaze].level;
			Lives = gameData.state.mazes[CurrentMaze].lives;
		}
		
		public void ShowMainMenu()
		{
			Condition = GameCondition.MainMenu;
		}
		
		public void StartNewGame()
		{
			gameData.CreateNewMaze(CurrentMaze, 0);
			StartGame();
		}
		
		public void StartSavedGame()
		{
			StartGame();
		}
		
		public void StartNextLevel()
		{
			OnNextLevel();
			SetMazeProgress();
		}
		
		public void Replay()
		{
			OnReplay();
			SetMazeProgress();
		}
		
		private void StartGame()
		{
			Condition = GameCondition.Game;			
			SetMazeProgress();
		}
		
		public void SaveGame()
		{
			gameData.SaveStateData();
		}
		
		public bool IsSavedGameExist()
		{
			return gameData.state.mazes.ContainsKey(CurrentMaze);
		}
		
		public int GetSavedGameLevel()
		{
			return gameData.state.mazes[CurrentMaze].level;
		}
		
		public void Pause()
		{
			OnPause();
		}
		
		public void Resume()
		{
			OnResume();
		}
		
		public void CollectDot(MazeElementDefId dotType)
		{
			Scores += gameData.defs.mazeElements[((int)dotType).ToString()].scores;
		}
		
		public void CollectBonus()
		{
			Scores += gameData.defs.levels[Level].bonus.scores;
		}
		
		public void PacmanCatched()
		{
			--Lives;
		}
		
		public void EnemyCatched(int count)
		{
			Scores += (int)Mathf.Pow(2, (float)count) * gameData.defs.commonValues.enemyCatchScores;
		}
		
		public void Win()
		{
			Pause();
			OnGameEnd();
			
			if (!IsCurrentLevelLast(Level))
			{
				guiManager.ShowDialog(DialogType.Win);
				gameData.CreateNewMaze(CurrentMaze, ++Level);
			}
			else
			{
				guiManager.ShowDialog(DialogType.WinMaze);
				gameData.RemoveMaze(CurrentMaze);
			}
		}
		
		public bool IsCurrentLevelLast(int level)
		{
			return level == (gameData.defs.levels.Count - 1);
		}
		
		private void GameOver()
		{
			Pause();
			OnGameEnd();
			
			gameData.CreateNewMaze(CurrentMaze, Level);
			
			guiManager.ShowDialog(DialogType.Defeat);
		}
	}
}