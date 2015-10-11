using UnityEngine;
using System.Collections;

namespace Pacman.Model.Unit
{
	public class ClydeModel : EnemyModel
	{
		private const int pointOffset = 8;
		
		public ClydeModel()
		{
			UnitId = UnitDefId.Clyde;
			
			Init();
		}
		
		protected override void Init()
		{
			base.Init();
		}
		
		protected override Vector2 GetChasePoint()
		{
			var distance = Vector2.Distance(pacmanPosition.point, CurrentPosition.point);
			
			if (distance > pointOffset)
				return pacmanPosition.point;
			else
				return scatterPosition;
		}
	}
}