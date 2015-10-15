using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.Units
{
	public class EnemyMediator : UnitMediator
	{
		public void SetFrighteningBehavior(float time)
		{
			((EnemyView)view).SetFrighteningBehavior(time);
		}
		
		public void StopFrighteningBehavior()
		{
			((EnemyView)view).StopFrighteningBehavior();
		}
		
		protected override void SetMaterial()
		{
			base.SetMaterial();
			((EnemyView)view).SaveNormalColor();
		}
		
		protected override void CollisionDetected(string otherUnitId)
		{
			// режим "боязни", пакман заловил привидение
			if (model.UnitBehaviorMode == UnitBehaviorMode.frightening && otherUnitId == UnitDefId.Pacman)
				model.Catch();
		}
	}
}