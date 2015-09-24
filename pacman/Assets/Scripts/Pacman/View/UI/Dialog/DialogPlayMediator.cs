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
			guiManager.ShowDialog(DialogType.Settings);
		}
		
		private void ContinueGame()
		{
			gameController.StartGame();
		}
	}
}