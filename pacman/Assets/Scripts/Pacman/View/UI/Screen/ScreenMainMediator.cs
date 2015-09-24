using UnityEngine;
using System.Collections;

using Pacman.Logic;
using Pacman.Model;

namespace Pacman.View.UI.Screen
{
	public class ScreenMainMediator : MonoBehaviour
	{
		[SerializeField]
		private ScreenMainView view;
		
		private GameData gameData = GameData.gameData;
		
		private GuiManager guiManager = GuiManager.guiManager;
		
		private GameController gameController = GameController.gameController;
		
		void Awake()
		{
			CreateMazeButtons();
			
			view.OnMazeChosen += MazeChosen;
		}
		
		void OnDestroy()
		{
			view.OnMazeChosen -= MazeChosen;
		}
		
		private void CreateMazeButtons()
		{
			foreach (var maze in gameData.defs.levels)
				view.CreateMazeButton(maze.Key, maze.Value.name);
		}
		
		private void MazeChosen(string mazeId)
		{
			gameController.CurrentMaze = mazeId;
			
			guiManager.ShowDialog(DialogType.Play);
		}
	}
}