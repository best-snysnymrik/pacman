using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Dialog
{
	public class DialogPauseView : MonoBehaviour
	{
		[SerializeField]
		private Button exitButton;
		[SerializeField]
		private Button continueButton;
		
		public delegate void ButtonPressedHandler();
		public event ButtonPressedHandler OnExitPressed;
		public event ButtonPressedHandler OnContinuePressed;
		
		void Awake()
		{
			exitButton.onClick.AddListener(() => OnExitPressed());
			continueButton.onClick.AddListener(() => OnContinuePressed());
		}
		
		void OnDestroy()
		{
			exitButton.onClick.RemoveAllListeners();
			continueButton.onClick.RemoveAllListeners();
		}
	}
}