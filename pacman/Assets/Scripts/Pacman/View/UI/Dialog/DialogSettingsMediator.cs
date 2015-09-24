using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Dialog
{
	public class DialogSettingsMediator : MonoBehaviour
	{
		[SerializeField]
		private DialogSettingsView view;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		void Awake()
		{
			view.OnClosePressed += Close;
			view.OnPlayPressed += Play;
		}
		
		void OnDestroy()
		{
			view.OnClosePressed -= Close;
			view.OnPlayPressed -= Play;
		}
		
		private void Close()
		{
			guiManager.CloseDialog(gameObject);
		}
		
		private void Play()
		{
			gameController.StartGame();
		}
	}
}