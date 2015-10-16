using UnityEngine;
using System.Collections.Generic;

namespace Pacman.View.Units.Behavior
{
	public class ColorPeriod
	{
		public float time;
		public Color color;
		
		public ColorPeriod(float time, Color color)
		{
			this.time = time;
			this.color  = color;
		}
	}
	
	public class FrighteningBehavior : MonoBehaviour
	{
		private Color frighteningColor = new Color(0.016f, 0.207f, 0.773f, 1f);
		private Color normalColor;
		
		private const float flashTime = 0.15f;
		private const int flashCount = 5;
		
		private Queue<ColorPeriod> colorPeriods = new Queue<ColorPeriod>();
		private ColorPeriod currentColorPeriod;
		
		private bool isFrighteningMode = false;
		
		void Awake()
		{}
		
		public void SaveNormalColor()
		{
			normalColor = renderer.material.GetColor("_Color");
		}
		
		public void StartFrighteningBehavior(float time)
		{
			CreateColorQueue(time);
			ApplyNextColorPeriod();
			
			isFrighteningMode = true;
		}
		
		private void CreateColorQueue(float time)
		{
			colorPeriods.Enqueue(new ColorPeriod(time - 2 * flashCount * flashTime, frighteningColor));
			
			for (int i = 0; i < flashCount; ++i)
			{
				colorPeriods.Enqueue(new ColorPeriod(flashTime, normalColor));
				colorPeriods.Enqueue(new ColorPeriod(flashTime, frighteningColor));
			}
		}
		
		private void ApplyNextColorPeriod()
		{
			if (colorPeriods.Count == 0)
				currentColorPeriod = null;
			else
			{
				currentColorPeriod = colorPeriods.Dequeue();
				renderer.material.SetColor("_Color", currentColorPeriod.color);
			}
		}
		
		public void StopFrighteningBehavior()
		{
			isFrighteningMode = false;
			colorPeriods.Clear();
			renderer.material.SetColor("_Color", normalColor);			
		}
		
		void FixedUpdate()
		{
			if (!isFrighteningMode)
				return;
			
			if (currentColorPeriod == null)
			{
				isFrighteningMode = false;
				renderer.material.SetColor("_Color", normalColor);
				return;
			}

			currentColorPeriod.time -= Time.deltaTime;
			
			if (currentColorPeriod.time <= 0)
				ApplyNextColorPeriod();
		}
	}
}