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
			view.OnSaveAndExitGamePressed += SaveAndExitPressed;
			view.OnResumePressed += ResumePressed;
		}
		
		void OnDestroy()
		{
			view.OnSaveAndExitGamePressed -= SaveAndExitPressed;
			view.OnResumePressed -= ResumePressed;
		}
		
		private void SaveAndExitPressed()
		{
			gameController.SaveGame();
			gameController.ShowMainMenu();
		}
		
		private void ResumePressed()
		{
			gameController.Resume();
			guiManager.CloseDialog(gameObject);
		}
	}
}