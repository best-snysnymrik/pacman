using UnityEngine;
using System.Collections;

using Pacman.Model;
using Pacman.Logic;

namespace Pacman.View.Maze
{
	public class MazeMediator : MonoBehaviour
	{
		[SerializeField]
		private MazeView view;
		
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		void Awake()
		{
			CreateMaze();
		}
		
		private void CreateMaze()
		{
			var mazeDef = gameData.defs.levels[gameController.CurrentMaze].view;
			
			var elements = mazeDef.elements;
			var cols = mazeDef.columns;
			var rows = mazeDef.rows;
			
			for (int i = 0; i < elements.Count; ++i)
			{
				if (elements[i] == (int)MazeElementDefId.empty)
					continue;
				
				var xzPosition = new Vector2(i / cols, i % cols);
				
				if (elements[i] != (int)MazeElementDefId.wall)
					view.CreateElement((int)MazeElementDefId.floor, xzPosition);
				
				if (elements[i] > (int)MazeElementDefId.floor)
					view.CreateElement(elements[i], xzPosition);
			}
			
			view.SetMazePosition(new Vector3(-(float)rows / 2f, 0, -(float)cols / 2f));
		}
	}
}