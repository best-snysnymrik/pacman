using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{	
	public class FakePinkyModel : PinkyModel
	{
		public FakePinkyModel()
		{
			base.Init();
		}
		
		public void SetCurrentPosition(UnitPosition position)
		{
			CurrentPosition = position;
		}
		
		public Vector2 GetPinkyChasePoint()
		{
			return base.GetChasePoint();
		}
	}
	
	[TestFixture]
	public class PinkyModelTest
	{
		TestContext context;
		
		public PinkyModelTest()
		{
			context = new TestContext();
		}
			
		[Test]
		public void testGetPinkyChasePoint_PacmanInLeftDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(0, 10), Direction.left));			
			var result = context.pinky.GetPinkyChasePoint();
			
			Assert.AreEqual(new Vector2(0, 6), result);
		}
		
		[Test]
		public void testGetPinkyChasePoint_PacmanInDownDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(0, 12), Direction.down));			
			var result = context.pinky.GetPinkyChasePoint();
			
			Assert.AreEqual(new Vector2(4, 12), result);
		}
		
		[Test]
		public void testGetPinkyChasePoint_PacmanInRightDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(0, 27), Direction.right));			
			var result = context.pinky.GetPinkyChasePoint();
			
			Assert.AreEqual(new Vector2(0, 31), result);
		}
		
		[Test]
		public void testGetPinkyChasePoint_PacmanInUpDirection()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(1, 0), Direction.up));			
			var result = context.pinky.GetPinkyChasePoint();
			
			Assert.AreEqual(new Vector2(-3, 0), result);
		}
	}
}