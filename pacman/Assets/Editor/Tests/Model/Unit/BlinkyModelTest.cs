using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	public class FakeBlinkyModel : BlinkyModel
	{
		public FakeBlinkyModel()
		{
			base.Init();
		}
		
		public void SetCurrentPosition(UnitPosition position)
		{
			CurrentPosition = position;
		}
		
		public Vector2 GetBlinkyChasePoint()
		{
			return base.GetChasePoint();
		}
	}
	
	[TestFixture]
	public class BlinkyModelTest
	{
		TestContext context;
		
		public BlinkyModelTest()
		{
			context = new TestContext();
		}
			
		[Test]
		public void testGetBlinkyChasePoint()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 11), Direction.left));
			var result = context.blinky.GetBlinkyChasePoint();
			
			Assert.AreEqual(new Vector2(11, 11), result);
		}
	}
}