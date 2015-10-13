using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Dialog
{
	public class DialogPlayMediator : MonoBehaviour
	{
		[SerializeField]
		private DialogPlayView view;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		void Awake()
		{
			view.OnClosePressed += Close;
			view.OnNewGamePressed += NewGame;
			view.OnContinueGamePressed += ContinueGame;
			
			if (!gameController.IsSavedGameExist())
				view.HideContinueGameButton();
		}
		
		void OnDestroy()
		{
			view.OnClosePressed -= Close;
			view.OnNewGamePressed -= NewGame;
			view.OnContinueGamePressed -= ContinueGame;
		}
		
		private void Close()
		{
			guiManager.CloseDialog(gameObject);
		}
		
		private void NewGame()
		{
			gameController.StartNewGame();
		}
		
		private void ContinueGame()
		{
			gameController.ContinueGame();
		}
	}
}