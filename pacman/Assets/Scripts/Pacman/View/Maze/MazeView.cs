using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pacman.Model;

namespace Pacman.View.Maze
{
	public class MazeView : MonoBehaviour
	{
		[Serializable]
		private class MazeElement
		{
			public MazeElementDefId id = MazeElementDefId.empty;
			public GameObject prefab = null;
		};
		
		[SerializeField]
		private GameObject elementsContainer;
		[SerializeField]
		private List<MazeElement> mazeElements;
		
		public void CreateElement(int id, Vector2 xzPosition)
		{
			var instance = Instantiate(mazeElements.Find(x => (int)x.id == id).prefab) as GameObject;
			instance.transform.SetParent(elementsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
		}
		
		public void SetMazePosition(Vector3 position)
		{
			elementsContainer.transform.position = position;
		}
	}

}