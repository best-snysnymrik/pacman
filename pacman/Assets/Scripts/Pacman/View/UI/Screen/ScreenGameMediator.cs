using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.UI.Screen
{
	public class ScreenGameMediator : MonoBehaviour
	{
		[SerializeField]
		private ScreenGameView view;
		
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
			guiManager.ShowDialog(DialogType.Pause);
		}
	}
}