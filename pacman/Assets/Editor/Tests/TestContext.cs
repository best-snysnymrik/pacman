using UnityEngine;
using System.Collections;

using Pacman;
using Pacman.Data;
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
	}
	
	public class TestContext
	{
		public GameData gameData = GameData.gameData;
		public GameController gameController = GameController.gameController;
		
		public GameModel gameModel;
		public MazeModel mazeModel;
		
		public FakePacmanModel pacman;
		public FakeBlinkyModel blinky;
		public FakePinkyModel pinky;
		public FakeInkyModel inky;
		public FakeClydeModel clyde;
		
		public TestContext()
		{
			gameData.OnDataInitialized += DataInitialized;
			
			EntryPoint entry = GameObject.FindObjectOfType<EntryPoint>();
			gameData.Init(entry, true);
			gameData.LoadData();
			
			InitUnits();
		}
		
		private void InitUnits()
		{
			gameController.CurrentMaze = "0";
			gameData.state.mazes[gameController.CurrentMaze].level = 0;
			
			gameModel = new GameModel();
			mazeModel = new MazeModel();
			
			gameModel.AddUnit(pacman = new FakePacmanModel());
			gameModel.AddUnit(blinky = new FakeBlinkyModel());			
			gameModel.AddUnit(pinky = new FakePinkyModel());
			gameModel.AddUnit(inky = new FakeInkyModel());
			gameModel.AddUnit(clyde = new FakeClydeModel());
			
			((UnitModel)pacman).SetMazeModel(mazeModel);
			((UnitModel)blinky).SetMazeModel(mazeModel);
			((UnitModel)pinky).SetMazeModel(mazeModel);
			((UnitModel)inky).SetMazeModel(mazeModel);
			((UnitModel)clyde).SetMazeModel(mazeModel);
			
			gameModel.SetObservation();
		}
		
		private void DataInitialized(){}
	}
}