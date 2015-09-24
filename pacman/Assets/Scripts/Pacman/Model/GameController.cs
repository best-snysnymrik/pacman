using UnityEngine;
using System.Collections;

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
		
		public void StartGame()
		{
			Condition = GameCondition.Game;			
		}
		
		public void ShowMainMenu()
		{
			Condition = GameCondition.MainMenu;
		}
	}
}