using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	[TestFixture]
	public class GameModelTest
	{
		TestContext context;
		
		public GameModelTest()
		{
			context = new TestContext();
		}
	}
}