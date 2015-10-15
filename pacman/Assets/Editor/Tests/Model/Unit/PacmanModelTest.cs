using UnityEngine;
using System.Collections;

using NUnit.Framework;
using NSubstitute;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Tests
{
	public class FakePacmanModel : PacmanModel
	{
		public FakePacmanModel()
		{
			base.Init();
		}
		
		public void SetCurrentPosition(UnitPosition position)
		{
			CurrentPosition = position;
		}
		
		public UnitPosition GetCurrentPosition()
		{
			return CurrentPosition;
		}
	}
	
	[TestFixture]
	public class PacmanModelTest
	{
	}
}