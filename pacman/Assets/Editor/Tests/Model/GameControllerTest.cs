using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{	
	[TestFixture]
	public class GameControllerTest
	{
		TestContext context;
		
		public GameControllerTest ()
		{
			context = new TestContext ();
		}
		
		[Test]
		public void testCollectDot_Dot_ScoreCount()
		{
			var scores = context.gameController.Scores;
			
			context.gameController.CollectDot(MazeElementDefId.dot);
			
			Assert.AreEqual(scores + 10, context.gameController.Scores);
		}
		
		[Test]
		public void testCollectDot_Energizer_ScoreCount()
		{
			var scores = context.gameController.Scores;
			
			context.gameController.CollectDot(MazeElementDefId.energizer);
			
			Assert.AreEqual(scores + 50, context.gameController.Scores);
		}
		
		[Test]
		public void testCollectBonus_ScoreCount()
		{
			var scores = context.gameController.Scores;
			
			context.gameController.CollectBonus();
			
			Assert.AreEqual(scores + 100, context.gameController.Scores);
		}
		
		[Test]
		public void testEnemyCatched_1()
		{
			var scores = context.gameController.Scores;
			
			context.gameController.EnemyCatched(0);
			
			Assert.AreEqual(scores + 200, context.gameController.Scores);
		}
		
		[Test]
		public void testEnemyCatched_3()
		{
			var scores = context.gameController.Scores;
			
			context.gameController.EnemyCatched(3);
			
			Assert.AreEqual(scores + 1600, context.gameController.Scores);
		}
		
		[Test]
		public void testIsCurrentLevelLast_Last()
		{
			var result = context.gameController.IsCurrentLevelLast(2);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testIsCurrentLevelLast_NotLast()
		{
			var result = context.gameController.IsCurrentLevelLast(1);
			
			Assert.IsFalse(result);
		}
	}
}