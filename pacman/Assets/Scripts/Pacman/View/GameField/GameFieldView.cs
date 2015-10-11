using UnityEngine;
using System.Collections.Generic;

namespace Pacman.View.GameField
{
	public class GameFieldView : MonoBehaviour
	{
		[SerializeField]
		private GameObject elementsContainer;
		[SerializeField]
		private GameObject unitsContainer;
		
		public void CreateMazeElement(string prefabName, Vector2 xzPosition)
		{
			var instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
			instance.transform.SetParent(elementsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
		}
		
		public GameObject CreateUnit(string prefabName, Vector2 xzPosition)
		{
			var instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
			instance.transform.SetParent(unitsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
			
			return instance;
		}
		
		public void SetCameraPosition(Vector3 position)
		{
			if (Camera.main != null)
				Camera.main.transform.position += position;
		}
	}
}