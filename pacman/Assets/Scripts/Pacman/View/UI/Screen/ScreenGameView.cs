using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Screen
{
	public class ScreenGameView : MonoBehaviour
	{
		[SerializeField]
		private Button pauseButton;
		
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
	}
}