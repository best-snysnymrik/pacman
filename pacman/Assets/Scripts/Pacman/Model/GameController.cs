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
		
		private GameData gameData = GameData.gameData;
		
		
		public void SetSceneView(SceneView view)
		{
			this.view = view;
		}
		
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
		
		public string CurrentMaze
		{
			get;
			set;
		}
		
		private void ChangeScene(GameCondition condition)
		{
			view.ClearScene();
			view.ShowScene(condition);
		}
		
		public void ShowMainMenu()
		{
			Condition = GameCondition.MainMenu;
		}
		
		public void StartNewGame()
		{
			gameData.CreateNewMaze(CurrentMaze);
			Condition = GameCondition.Game;			
		}
		
		public void SaveGame()
		{
			gameData.SaveStateData();
		}
		
		public void ContinueGame()
		{
			Condition = GameCondition.Game;			
		}
		
		public bool IsSavedGameExist()
		{
			return gameData.state.mazes.ContainsKey(CurrentMaze);
		}
		
		public void Pause()
		{
			OnPause();
		}
		
		public void Resume()
		{
			OnResume();
		}
	}
}