using UnityEngine;
using System.Collections.Generic;

using Pacman.Model;
using Pacman.View.UI.Widget;

namespace Pacman.View.UI.Screen
{
	public class ScreenMainView : MonoBehaviour
	{
		[SerializeField]
		private GameObject mazeButtonsContainer;
		[SerializeField]
		private GameObject mazeButtonPrefab;
		
		private List<WidgetMazeChooseButton> widgets = new List<WidgetMazeChooseButton>();
		
		public delegate void MazeChosenHandler(string mazeId);
		public event MazeChosenHandler OnMazeChosen;
		
		public void CreateMazeButton(string mazeId, string mazeName)
		{
			var instance = Instantiate(mazeButtonPrefab) as GameObject;
			instance.transform.SetParent(mazeButtonsContainer.transform);
			
			var widget = instance.GetComponent<WidgetMazeChooseButton>();
			widget.SetData(mazeName);
			widget.button.onClick.AddListener(() => OnMazeChosen(mazeId));
			
			widgets.Add(widget);
		}
		
		void OnDestroy()
		{
			foreach (var widget in widgets)
				widget.button.onClick.RemoveAllListeners();
		}
	}
}