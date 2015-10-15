using UnityEngine;
using System.Collections;

using Pacman.Model;

namespace Pacman.View.Units
{
	public class PacmanMediator : UnitMediator
	{
		protected override void CollisionDetected(string otherUnitId)
		{
			// пакмана заловили
			if (model.UnitBehaviorMode == UnitBehaviorMode.normal)
				model.Catch();
		}
	}
}