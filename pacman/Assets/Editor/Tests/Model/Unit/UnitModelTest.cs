using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	[TestFixture]
	public class UnitModelTest
	{
		TestContext context;
		
		public UnitModelTest()
		{
			context = new TestContext();
		}
		
		[Test]
		public void testCalculateSpeed_PacmanInFrighteningMode()
		{
			context.gameModel.UnitsBehaviorMode = UnitBehaviorMode.frightening;
			
			var result = context.pacman.Speed;
			
			Assert.AreEqual(1.8f, result);
		}
		
		[Test]
		public void testCalculateSpeed_BlinkyInFrighteningMode()
		{
			context.gameModel.UnitsBehaviorMode = UnitBehaviorMode.frightening;
			
			var result = context.blinky.Speed;
			
			Assert.AreEqual(1f, result);
		}
		
		[Test]
		public void testCalculateSpeed_BlinkyInNormalMode()
		{
			context.gameModel.UnitsBehaviorMode = UnitBehaviorMode.normal;
			
			var result = context.blinky.Speed;
			
			Assert.AreEqual(1.5f, result);
		}
	}
}