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
		public void testIsPortPoint_InPortPoint()
		{
			Vector2 point = new Vector2(14, 0);
			
			var result = context.mazeModel.IsPortPoint(point);
			
			Assert.IsTrue(result);
		}
		
		[Test]
		public void testIsPortPoint_NotInPortPoint()
		{
			Vector2 point = new Vector2(14, 20);
			
			var result = context.mazeModel.IsPortPoint(point);
			
			Assert.IsFalse(result);
		}
	}
}