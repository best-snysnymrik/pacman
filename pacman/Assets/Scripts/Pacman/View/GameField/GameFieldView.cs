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
		
		private Dictionary<int, GameObject> dots = new Dictionary<int, GameObject>();
		private GameObject bonus;
		
		private List<GameObject> units = new List<GameObject>();
		
		
		public GameObject CreateMazeElement(string prefabName, Vector2 xzPosition)
		{
			var instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
			instance.transform.SetParent(elementsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
			
			return instance;
		}
		
		public GameObject CreateUnit(string prefabName, Vector2 xzPosition)
		{
			var instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
			instance.transform.SetParent(unitsContainer.transform);			
			instance.transform.position = new Vector3(xzPosition.x, instance.transform.position.y, xzPosition.y);
			
			units.Add(instance);
			
			return instance;
		}
		
		public void DestroyUnits()
		{
			foreach (var unit in units)
				DestroyObject(unit);
		}
		
		public void CreateDot(int index, string prefabName, Vector2 xzPosition)
		{
			var dot = CreateMazeElement(prefabName, xzPosition);
			dots[index] = dot;
		}
		
		public void DestroyDot(int index)
		{
			if (dots.ContainsKey(index))
				DestroyObject(dots[index]);
		}
		
		public void DestroyDots()
		{
			foreach (var dot in dots.Values)
				DestroyObject(dot);
			
			dots.Clear();
		}
		
		public void CreateBonus(string prefabName, Vector2 xzPosition)
		{
			bonus = CreateMazeElement(prefabName, xzPosition);
		}
		
		public void DestroyBonus()
		{
			DestroyObject(bonus);
		}
		
		public void SetCameraPosition(Vector3 position)
		{
			if (Camera.main != null)
				Camera.main.transform.position += position;
		}
	}
}