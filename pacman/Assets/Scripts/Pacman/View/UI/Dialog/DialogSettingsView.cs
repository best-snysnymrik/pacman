using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Dialog
{
	public class DialogSettingsView : MonoBehaviour
	{
		[SerializeField]
		private Button closeButton;
		[SerializeField]
		private Button playButton;
		
		public delegate void ButtonPressedHandler();
		public event ButtonPressedHandler OnClosePressed;
		public event ButtonPressedHandler OnPlayPressed;
		
		void Awake()
		{
			closeButton.onClick.AddListener(() => OnClosePressed());
			playButton.onClick.AddListener(() => OnPlayPressed());
		}
		
		void OnDestroy()
		{
			closeButton.onClick.RemoveAllListeners();
			playButton.onClick.RemoveAllListeners();
		}
	}
}