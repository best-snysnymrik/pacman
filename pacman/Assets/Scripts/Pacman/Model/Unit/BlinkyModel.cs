using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Model.Unit
{
	public class BlinkyModel : EnemyModel
	{		
		public BlinkyModel()
		{
			UnitId = UnitDefId.Blinky;
			
			Init();
		}
		
		protected override void Init()
		{
			base.Init();
		}
						
		protected override Vector2 GetChasePoint()
		{
			return pacmanPosition.point;
		}
	}
}