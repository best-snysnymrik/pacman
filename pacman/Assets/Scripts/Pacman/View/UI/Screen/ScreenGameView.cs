using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Pacman.View.UI.Screen
{
	public class ScreenGameView : MonoBehaviour
	{
		[SerializeField]
		private Button pauseButton;
		[SerializeField]
		private Text scores;
		[SerializeField]
		private List<GameObject> lives;
		
		public delegate void PausePressedHandler();
		public event PausePressedHandler OnPausePressed;
		
		void Awake()
		{
			pauseButton.onClick.AddListener(() => OnPausePressed());
		}
		
		void OnDestroy()
		{
			pauseButton.onClick.RemoveAllListeners();
		}
		
		public void SetScoresCount(int scoresCount)
		{
			scores.text = scoresCount.ToString();
		}
		
		public void SetLivesCount(int livesCount)
		{
			int counter = 0;
			
			foreach (var live in lives)
			{
				if (counter < livesCount)
					live.SetActive(true);
				else
					live.SetActive(false);
				
				++counter;
			}
		}
	}
}