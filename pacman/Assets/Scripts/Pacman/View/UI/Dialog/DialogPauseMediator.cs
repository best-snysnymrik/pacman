using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Dialog
{
	public class DialogPauseMediator : MonoBehaviour
	{
		[SerializeField]
		private DialogPauseView view;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		void Awake()
		{
			view.OnExitPressed += ExitPressed;
			view.OnContinuePressed += ContinuePressed;
		}
		
		void OnDestroy()
		{
			view.OnExitPressed -= ExitPressed;
			view.OnContinuePressed -= ContinuePressed;
		}
		
		private void ExitPressed()
		{
			gameController.SaveGame();
			gameController.ShowMainMenu();
		}
		
		private void ContinuePressed()
		{
			gameController.Resume();
			guiManager.CloseDialog(gameObject);
		}
	}
}