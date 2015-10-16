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
			
			gameController.OnScoresChanged += SetScoresCount;
			gameController.OnLivesChanged += LivesCountChanged;
			
			SetScoresCount(gameController.Scores);
			SetScoresCount(gameController.Lives);
		}
		
		void OnDestroy()
		{
			view.OnPausePressed -= PausePressed;
			
			gameController.OnScoresChanged -= SetScoresCount;
			gameController.OnLivesChanged -= LivesCountChanged;
		}		
		
		private void PausePressed()
		{
			gameController.Pause();
			guiManager.ShowDialog(DialogType.Pause);
		}
		
		private void SetScoresCount(int scoresCount)
		{
			view.SetScoresCount(scoresCount);
		}
		
		private void LivesCountChanged(int count)
		{
		}
	}
}