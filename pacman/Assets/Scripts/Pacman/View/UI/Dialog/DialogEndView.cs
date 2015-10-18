using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Dialog
{
	public class DialogEndView : MonoBehaviour
	{
		[SerializeField]
		private Button exitButton;
		[SerializeField]
		private Button nextGameButton;
		[SerializeField]
		private Button replayButton;
		[SerializeField]
		private Text titleText;
		[SerializeField]
		private GameObject mazeFinishedText;
		
		public delegate void ButtonPressedHandler();
		public event ButtonPressedHandler OnExitPressed;
		public event ButtonPressedHandler OnNextGamePressed;
		public event ButtonPressedHandler OnReplayPressed;
		
		void Awake()
		{
			exitButton.onClick.AddListener(() => OnExitPressed());
			nextGameButton.onClick.AddListener(() => OnNextGamePressed());
			replayButton.onClick.AddListener(() => OnReplayPressed());
		}
		
		void OnDestroy()
		{
			exitButton.onClick.RemoveAllListeners();
			nextGameButton.onClick.RemoveAllListeners();
			replayButton.onClick.RemoveAllListeners();
		}
		
		public void SetTitleText(string text)
		{
			titleText.text = text;
		}
		
		public void ShowMazeFinishedText()
		{
			mazeFinishedText.SetActive(true);
		}
		
		public void HideNextGameButton()
		{
			nextGameButton.gameObject.SetActive(false);
		}
		
		public void HideReplayGameButton()
		{
			replayButton.gameObject.SetActive(false);
		}
	}
}