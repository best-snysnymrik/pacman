using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Dialog
{
	public class DialogPauseView : MonoBehaviour
	{
		[SerializeField]
		private Button saveAndExitButton;
		[SerializeField]
		private Button resumeButton;
		
		public delegate void ButtonPressedHandler();
		public event ButtonPressedHandler OnSaveAndExitGamePressed;
		public event ButtonPressedHandler OnResumePressed;
		
		void Awake()
		{
			saveAndExitButton.onClick.AddListener(() => OnSaveAndExitGamePressed());
			resumeButton.onClick.AddListener(() => OnResumePressed());
		}
		
		void OnDestroy()
		{
			saveAndExitButton.onClick.RemoveAllListeners();
			resumeButton.onClick.RemoveAllListeners();
		}
	}
}