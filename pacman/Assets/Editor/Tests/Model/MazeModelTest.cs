using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	[TestFixture]
	public class MazeModelTest
	{
		TestContext context;
		
		public MazeModelTest()
		{
			context = new TestContext();
		}
		
		[Test]
		public void testStepIsPossible_PossibleStep()
		{
			Vector2 point = new Vector2(23, 13);
			
			var result = context.mazeModel.StepIsPossible(point, Direction.left);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testStepIsPossible_ImpossibleStep()
		{
			Vector2 point = new Vector2(23, 13);
			
			var result = context.mazeModel.StepIsPossible(point, Direction.up);
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void testStepIsPossible_BoundaryImpossibleStep()
		{
			Vector2 point = new Vector2(14, 0);
			
			var result = context.mazeModel.StepIsPossible(point, Direction.left);
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void testStepIsPossible_DoorImpossibleStep()
		{
			Vector2 point = new Vector2(11, 13);
			
			var result = context.mazeModel.StepIsPossible(point, Direction.down);
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void testIsPointOfType_IsPortPoint()
		{
			Vector2 point = new Vector2(14, 0);
			
			var result = context.mazeModel.IsPointOfType(point, MazeElementDefId.port);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testIsPointOfType_NotIsPortPoint()
		{
			Vector2 point = new Vector2(14, 20);
			
			var result = context.mazeModel.IsPointOfType(point, MazeElementDefId.port);
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void testIsPointOfType_IsDotPoint()
		{
			Vector2 point = new Vector2(1, 2);
			
			var result = context.mazeModel.IsPointOfType(point, MazeElementDefId.dot);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testIsPointOfType_NotIsDotPoint()
		{
			Vector2 point = new Vector2(3, 1);
			
			var result = context.mazeModel.IsPointOfType(point, MazeElementDefId.dot);
			
			Assert.IsFalse(result);
		}
		
		[Test]
		public void testIsPointOfType_IsEnergizerPoint()
		{
			Vector2 point = new Vector2(3, 1);
			
			var result = context.mazeModel.IsPointOfType(point, MazeElementDefId.energizer);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testCollectEnergizer_DotCount()
		{
			Vector2 point = new Vector2(3, 26);
			context.mazeModel.DotCount = 2;
			
			context.mazeModel.CollectDot(point, MazeElementDefId.energizer);
			
			Assert.AreEqual(3, context.mazeModel.DotCount);
		}
		
		[Test]
		public void testCollectEnergizer_MazeElementChanged()
		{
			Vector2 point = new Vector2(3, 26);
			var index = (int)(point.x * context.mazeModel.Cols + point.y);
						
			context.mazeModel.CollectDot(point, MazeElementDefId.energizer);
			
			Assert.AreEqual((int)MazeElementDefId.floor, context.gameData.state.mazes[context.gameController.CurrentMaze].elements[index]);
		}
		
		[Test]
		public void testCollectDot_DotCount()
		{
			Vector2 point = new Vector2(2, 26);
			context.mazeModel.DotCount = 12;
			
			context.mazeModel.CollectDot(point, MazeElementDefId.dot);
			
			Assert.AreEqual(13, context.mazeModel.DotCount);
		}
		
		[Test]
		public void testCollectDot_NotDotPoint()
		{
			Vector2 point = new Vector2(11, 10);
			context.mazeModel.DotCount = 12;
			
			context.mazeModel.CollectDot(point, MazeElementDefId.dot);
			
			Assert.AreEqual(12, context.mazeModel.DotCount);
		}
	}
}