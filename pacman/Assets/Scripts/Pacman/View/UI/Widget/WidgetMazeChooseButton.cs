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
		[SerializeField]
		private RawImage mazeIcon;
		
		public void SetData(string name, string iconPath)
		{
			mazeName.text = name;
			
			mazeIcon.texture = Instantiate(Resources.Load(iconPath, typeof(Texture))) as Texture;
		}
	}
}