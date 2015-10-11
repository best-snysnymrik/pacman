using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	[TestFixture]
	public class EnemyModelTest
	{
		TestContext context;
		
		public EnemyModelTest()
		{
			context = new TestContext();
		}
		
		[Test]
		public void testGetNextMovePoint_PinkyInPortPoint()
		{
			context.pinky.SetCurrentPosition(new UnitPosition(new Vector2(14, 0), Direction.left));
			
			var result = context.pinky.GetNextMovePoint();
			
			Assert.AreEqual(new Vector2(14, 26), result.point);
			Assert.AreEqual(Direction.left, result.direction);
		}
		
		[Test]
		public void testGetNextMovePointToTarget_ChaseMode()
		{
			context.pinky.EnemyBehaviorMode = EnemyBehaviorMode.chase;
			context.pinky.SetCurrentPosition(new UnitPosition(new Vector2(5, 6), Direction.left));
			
			var result = context.pinky.GetNextMovePoint();
			
			Assert.AreEqual(new Vector2(6, 6), result.point);
			Assert.AreEqual(Direction.down, result.direction);
		}
		
		[Test]
		public void testGetNextMovePointToTarget_ChangeModeFromChaseToScatter()
		{
			context.pinky.EnemyBehaviorMode = EnemyBehaviorMode.chase;
			context.pinky.SetCurrentPosition(new UnitPosition(new Vector2(5, 5), Direction.left));
			context.pinky.EnemyBehaviorMode = EnemyBehaviorMode.scatter;
			
			var result = context.pinky.GetNextMovePoint();
			
			Assert.AreEqual(new Vector2(5, 6), result.point);
			Assert.AreEqual(Direction.right, result.direction);
		}
		
		[Test]
		public void testGetNextMovePointToTarget_ScatterMode()
		{
			context.pinky.EnemyBehaviorMode = EnemyBehaviorMode.scatter;
			context.pinky.SetCurrentPosition(new UnitPosition(new Vector2(5, 6), Direction.left));
			
			var result = context.pinky.GetNextMovePoint();
			
			Assert.AreEqual(new Vector2(5, 5), result.point);
			Assert.AreEqual(Direction.left, result.direction);
		}
	}
}