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
		
		[Test]
		public void testCatch_SetPacmanToStartPoint()
		{
			context.pacman.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.left));
			
			context.pacman.Catch();
			
			Assert.AreEqual(new Vector2(23, 13), context.pacman.GetCurrentPosition().point);
		}
		
		[Test]
		public void testCatch_Pacman_DecrementLive()
		{
			var lives = context.gameController.Lives;
			
			context.pacman.Catch();
			
			Assert.AreEqual(lives - 1, context.gameController.Lives);
		}
		
		[Test]
		public void testCatch_SetBlinkyToStartPoint()
		{
			context.blinky.SetCurrentPosition(new UnitPosition(new Vector2(11, 10), Direction.left));
			
			context.blinky.Catch();
			
			Assert.AreEqual(new Vector2(11, 13), context.blinky.GetCurrentPosition().point);
		}
		
		[Test]
		public void testCatch_Blinky_IncrementScores()
		{
			var scores = context.gameController.Scores;
			
			context.blinky.Catch();
			
			Assert.AreEqual(scores + 200, context.gameController.Scores);
		}
	}
}