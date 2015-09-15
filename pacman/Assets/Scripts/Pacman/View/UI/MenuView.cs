using UnityEngine;
using System.Collections;

namespace Pacman.View.UI
{
	public class MenuView : MonoBehaviour
	{
		void Awake ()
		{
	
		}

		public void LoadMaze()
		{
			Application.LoadLevel("Game");
		}
	}
}