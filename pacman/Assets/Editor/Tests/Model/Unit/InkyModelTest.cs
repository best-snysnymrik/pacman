using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	public class FakeInkyModel : InkyModel
	{
		public FakeInkyModel()
		{
			base.Init();
		}
		
		public void SetCurrentPosition(UnitPosition position)
		{
			nextPosition = position;
			CurrentPosition = nextPosition;
		}
		
		public Vector2 GetInkyChasePoint()
		{
			return base.GetChasePoint();
		}
	}
	
	[TestFixture]
	public class InkyModelTest
	{
		TestContext context;
		
		public InkyModelTest()
		{
			context = new TestContext();
		}
			
		[Test]
		public void testGetInkyChasePoint_PacmanInLeftDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.left));
			context.blinky.SetCurrentPosition(new UnitPosition(new Vector2(11, 13), Direction.left));
			
			var result = context.inky.GetInkyChasePoint();
			
			Assert.AreEqual(new Vector2(11, 3), result);
		}
		
		[Test]
		public void testGetInkyChasePoint_PacmanInDownDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.down));
			context.blinky.SetCurrentPosition(new UnitPosition(new Vector2(14, 9), Direction.left));
			
			var result = context.inky.GetInkyChasePoint();
			
			Assert.AreEqual(new Vector2(12, 11), result);
		}
		
		[Test]
		public void testGetInkyChasePoint_PacmanInRightDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.right));
			context.blinky.SetCurrentPosition(new UnitPosition(new Vector2(14, 9), Direction.left));
			
			var result = context.inky.GetInkyChasePoint();
			
			Assert.AreEqual(new Vector2(8, 15), result);
		}
		
		[Test]
		public void testGetInkyChasePoint_PacmanInUpDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.up));
			context.blinky.SetCurrentPosition(new UnitPosition(new Vector2(12, 9), Direction.left));
			
			var result = context.inky.GetInkyChasePoint();
			
			Assert.AreEqual(new Vector2(6, 11), result);
		}
	}
}