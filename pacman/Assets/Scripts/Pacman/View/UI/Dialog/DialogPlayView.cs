using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Dialog
{
	public class DialogPlayView : MonoBehaviour
	{
		[SerializeField]
		private Button closeButton;
		[SerializeField]
		private Button newGameButton;
		[SerializeField]
		private Button continueGameButton;
		
		public delegate void ButtonPressedHandler();
		public event ButtonPressedHandler OnClosePressed;
		public event ButtonPressedHandler OnNewGamePressed;
		public event ButtonPressedHandler OnContinueGamePressed;
		
		void Awake()
		{
			closeButton.onClick.AddListener(() => OnClosePressed());
			newGameButton.onClick.AddListener(() => OnNewGamePressed());
			continueGameButton.onClick.AddListener(() => OnContinueGamePressed());
		}
		
		void OnDestroy()
		{
			closeButton.onClick.RemoveAllListeners();
			newGameButton.onClick.RemoveAllListeners();
			continueGameButton.onClick.RemoveAllListeners();
		}
		
		public void HideContinueGameButton()
		{
			continueGameButton.gameObject.SetActive(false);
		}
	}
}