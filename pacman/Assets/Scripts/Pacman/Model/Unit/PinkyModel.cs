using UnityEngine;
using System.Collections.Generic;

using Pacman.Model;

namespace Pacman.Model.Unit
{
	public class PinkyModel : EnemyModel
	{
		private const int pointOffset = 4;
		private Dictionary<Direction, Vector2> offsets = new Dictionary<Direction, Vector2>()
		{
			{Direction.left, new Vector2(0, -pointOffset)},
			{Direction.right, new Vector2(0, pointOffset)},
			{Direction.up, new Vector2(-pointOffset, 0)},
			{Direction.down, new Vector2(pointOffset, 0)}
		};
		
		public PinkyModel()
		{
			UnitId = UnitDefId.Pinky;
			
			Init();
		}
		
		protected override void Init()
		{
			base.Init();
		}
		
		protected override Vector2 GetChasePoint()
		{
			return pacmanPosition.point + offsets[pacmanPosition.direction];
		}
	}
}