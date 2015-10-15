using UnityEngine;
using System.Collections;

using Pacman.View.Units.Behavior;

namespace Pacman.View.Units
{
	public class EnemyView : UnitView
	{
		[SerializeField]
		private FrighteningBehavior frighteningBehavior;
		
		public void SetFrighteningBehavior(float time)
		{
			frighteningBehavior.StartFrighteningBehavior(time);
		}
		
		public void StopFrighteningBehavior()
		{
			frighteningBehavior.StopFrighteningBehavior();
		}
		
		public void SaveNormalColor()
		{
			frighteningBehavior.SaveNormalColor();
		}
	}
}