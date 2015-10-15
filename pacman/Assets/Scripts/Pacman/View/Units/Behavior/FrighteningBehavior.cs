using UnityEngine;
using System.Collections;

namespace Pacman.View.Units.Behavior
{
	public class FrighteningBehavior : MonoBehaviour
	{
		private Color frighteningColor = new Color(0.016f, 0.207f, 0.773f, 1f);
		private Color normalColor;
		
		private bool isFrighteningMode = false;
		private float frighteningBehaviorTimer = 0;
		
		void Awake()
		{}
		
		public void SaveNormalColor()
		{
			normalColor = renderer.material.GetColor("_Color");
		}
		
		public void StartFrighteningBehavior(float time)
		{
			frighteningBehaviorTimer = time;
			renderer.material.SetColor("_Color", frighteningColor);
			
			isFrighteningMode = true;
		}
		
		public void StopFrighteningBehavior()
		{
			renderer.material.SetColor("_Color", normalColor);
			
			isFrighteningMode = false;
		}
		
		void FixedUpdate()
		{
			if (!isFrighteningMode)
				return;
			
			frighteningBehaviorTimer -= Time.deltaTime;
			
			if (frighteningBehaviorTimer <= 0)
			{
				isFrighteningMode = false;
				renderer.material.SetColor("_Color", normalColor);
			}
		}
	}
}