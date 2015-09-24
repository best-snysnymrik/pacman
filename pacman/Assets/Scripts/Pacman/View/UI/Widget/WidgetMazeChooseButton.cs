using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pacman.View.UI.Widget
{
	public class WidgetMazeChooseButton : MonoBehaviour
	{
		[SerializeField]
		public Button button;
		[SerializeField]
		private Text mazeName;
		
		public void SetData(string name)
		{
			mazeName.text = name;
		}
	}
}