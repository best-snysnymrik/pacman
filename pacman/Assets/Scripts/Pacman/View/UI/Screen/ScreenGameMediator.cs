using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Screen
{
	public class ScreenGameMediator : MonoBehaviour
	{
		[SerializeField]
		private ScreenGameView view;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		void Awake()
		{
			view.OnPausePressed += PausePressed;
		}
		
		void OnDestroy()
		{
			view.OnPausePressed -= PausePressed;
		}
		
		private void PausePressed()
		{
			gameController.Pause();
			guiManager.ShowDialog(DialogType.Pause);
		}
	}
}