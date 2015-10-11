using UnityEngine;
using System.Collections.Generic;

namespace Pacman.Model.Unit
{
	public class InkyModel : EnemyModel
	{
		private UnitPosition blinkyPosition = null;
		
		private const int pointOffset = 2;
		private Dictionary<Direction, Vector2> offsets = new Dictionary<Direction, Vector2>()
		{
			{Direction.left, new Vector2(0, -pointOffset)},
			{Direction.right, new Vector2(0, pointOffset)},
			{Direction.up, new Vector2(-pointOffset, 0)},
			{Direction.down, new Vector2(pointOffset, 0)}
		};
		
		public InkyModel()
		{
			UnitId = UnitDefId.Inky;
			
			Init();
		}
		
		protected override void Init()
		{
			base.Init();
		}
		
		#region IObserver implementation
		public override void OnNext(UnitInfo info)
		{
			switch (info.id)
			{
				case UnitDefId.Pacman:
					pacmanPosition = info.position;
					break;
				case UnitDefId.Blinky:
					blinkyPosition = info.position;
					break;
			}
		}
		#endregion
		
		protected override Vector2 GetChasePoint()
		{
			if (blinkyPosition == null)
				return pacmanPosition.point;
			
			var blinky = pacmanPosition.point - blinkyPosition.point;
			var offset = pacmanPosition.point + 2 * offsets[pacmanPosition.direction];
			var result = offset + blinky;
			
			return result;
		}
	}
}