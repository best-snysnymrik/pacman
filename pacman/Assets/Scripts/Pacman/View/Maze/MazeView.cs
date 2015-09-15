using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pacman.Logic;
using Pacman.Model;

namespace Pacman.View.Maze
{
	public class MazeView : MonoBehaviour
	{
		[Serializable]
		private class MazeElement
		{
			public MazeElementDefId id;
			public GameObject prefab = null;
		};
		
		[SerializeField]
		GameObject elementsContainer;
		[SerializeField]
		List<MazeElement> mazeElements;
		
		private GameLogic logic;
		
		void Awake ()
		{
			logic = GameLogic.logic;
			
			CreateMaze();
		}
		
		private void CreateMaze()
		{
			var elements = logic.GetCurrentMaze().view.elements;
			var cols = logic.GetCurrentMaze().view.columns;
			var rows = logic.GetCurrentMaze().view.rows;
			
			for (int i = 0; i < elements.Count; ++i)
			{
				if (elements[i] == (int)MazeElementDefId.empty)
					continue;
				
				var xzPosition = new Vector2(i / cols, i % cols);
				
				if (elements[i] != (int)MazeElementDefId.wall)
					CreateElement((int)MazeElementDefId.floor, xzPosition);
				
				if (elements[i] > (int)MazeElementDefId.floor)
					CreateElement(elements[i], xzPosition);
			}
			
			elementsContainer.transform.position = new Vector3(-(float)rows / 2f, 0, -(float)cols / 2f);
		}
		
		private void CreateElement(int id, Vector2 xzPosition)
		{
			var instance = Instantiate(mazeElements.Find(x => (int)x.id == id).prefab) as GameObject;
			instance.transform.SetParent(elementsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
		}
	}

}