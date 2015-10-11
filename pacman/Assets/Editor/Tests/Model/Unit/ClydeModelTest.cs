using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	public class FakeClydeModel : ClydeModel
	{
		public FakeClydeModel()
		{
			base.Init();
		}
		
		public void SetCurrentPosition(UnitPosition position)
		{
			CurrentPosition = position;
		}
		
		public Vector2 GetClydeChasePoint()
		{
			return base.GetChasePoint();
		}
	}
	
	[TestFixture]
	public class ClydeModelTest
	{
		TestContext context;
		
		public ClydeModelTest()
		{
			context = new TestContext();
		}
			
		[Test]
		public void testGetClydeChasePoint_PacmanInArea()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.left));
			context.clyde.SetCurrentPosition(new UnitPosition(new Vector2(8, 12), Direction.left));
			var result = context.clyde.GetClydeChasePoint();
			
			Assert.AreEqual(new Vector2(30, 27), result);
		}
		
		[Test]
		public void testGetClydeChasePoint_PacmanOutOfArea()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.down));
			context.clyde.SetCurrentPosition(new UnitPosition(new Vector2(11, 19), Direction.left));
			var result = context.clyde.GetClydeChasePoint();
			
			Assert.AreEqual(new Vector2(11, 10), result);
		}
	}
}