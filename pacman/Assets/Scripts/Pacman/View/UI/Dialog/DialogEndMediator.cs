using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Dialog
{
	public class DialogEndMediator : MonoBehaviour
	{
		[SerializeField]
		private DialogEndView view;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		private const string defeatText = "ПОРАЖЕНИЕ!";
		private const string winText = "ПОБЕДА!";
		
		void Awake()
		{
			view.OnExitPressed += ExitPressed;
			view.OnNextGamePressed += NextGamePressed;
			view.OnReplayPressed += ReplayPressed;
		}
		
		void OnDestroy()
		{
			view.OnExitPressed -= ExitPressed;
			view.OnNextGamePressed -= NextGamePressed;
			view.OnReplayPressed -= ReplayPressed;
		}
		
		private void ExitPressed()
		{			
			gameController.ShowMainMenu();
		}
		
		private void NextGamePressed()
		{
			gameController.StartNextLevel();
			guiManager.CloseDialog(gameObject);
		}
		
		private void ReplayPressed()
		{
			gameController.Replay();
			guiManager.CloseDialog(gameObject);
		}
		
		public void SetDialogType(DialogType type)
		{
			switch (type)
			{
				case DialogType.Defeat:
					view.SetTitleText(defeatText);
					view.HideNextGameButton();
					break;
				case DialogType.Win:
					view.SetTitleText(winText);
					view.HideReplayGameButton();
					break;
				case DialogType.WinMaze:
					view.SetTitleText(winText);
					view.ShowMazeFinishedText();
					view.HideNextGameButton();
					view.HideReplayGameButton();
					break;
			}
		}
	}
}