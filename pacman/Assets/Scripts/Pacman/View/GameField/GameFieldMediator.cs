using UnityEngine;
using System.Collections;

using Pacman.Data;
using Pacman.Model;
using Pacman.Model.Unit;
using Pacman.View.Units;
using Pacman.View.Units.Inputt;

namespace Pacman.View.GameField
{
	public class GameFieldMediator : MonoBehaviour
	{
		[SerializeField]
		private GameFieldView view;
		
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		private GameModel gameModel;
		private MazeModel mazeModel;
		
		private Vector3 cameraOffset = Vector3.zero;
		
		private void Awake()
		{
			gameModel = new GameModel();
			mazeModel = new MazeModel();
			
			gameController.OnPause += gameModel.Pause;
			gameController.OnResume += gameModel.Resume;
			
			CreateMaze();
			CreateUnits();
			
			SetUpCamera();
			
			gameModel.StartGame();
		}
		
		private void FixedUpdate()
		{
			gameModel.FixedUpdate();
		}
		
		private void CreateMaze()
		{
			var elements = mazeModel.Elements;
			
			for (int i = 0; i < elements.Count; ++i)
			{
				int elementId = elements[i];
				
				if (elementId == (int)MazeElementDefId.empty)
					continue;
				
				var xzPosition = new Vector2(i / mazeModel.Cols, i % mazeModel.Cols);
				
				if (elementId != (int)MazeElementDefId.wall)
					view.CreateMazeElement(GetPrefabNameByMazeElementId((int)MazeElementDefId.floor), xzPosition);
				
				if (elementId > (int)MazeElementDefId.floor)
					view.CreateMazeElement(GetPrefabNameByMazeElementId(elementId), xzPosition);
			}
		}
		
		private string GetPrefabNameByMazeElementId(int id)
		{
			return gameData.defs.mazeElements[id.ToString()].prefab;
		}
		
		private void CreateUnits()
		{
			var units = gameData.state.mazes[gameController.CurrentMaze].units;
			
			foreach(var unit in units)
			{
				var xzPosition = new Vector2(unit.Value.position.x, unit.Value.position.y);
				
				GameObject unitObject = view.CreateUnit(gameData.defs.units[unit.Key].prefab, xzPosition);
								
				var unitMediator = unitObject.GetComponent<UnitMediator>();
				
				var unitModel = UnitModel.CreateUnitModel(unit.Key);
				unitModel.SetMediator(unitMediator);
				unitModel.SetMazeModel(mazeModel);
				
				gameModel.AddUnit(unitModel);
				
				if (unit.Key == UnitDefId.Pacman)
				{
					#if UNITY_EDITOR || UNITY_WEBPLAYER
					var inputController = gameObject.AddComponent<KeyboardInputController>();
					inputController.SetModel((PacmanModel)unitModel);
					#endif
					
					#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
					var inputController = gameObject.AddComponent<TouchInputController>();
					inputController.SetModel((PacmanModel)unitModel);
					#endif
				}
			}
		}
		
		private void SetUpCamera()
		{
			cameraOffset = new Vector3((float)mazeModel.Rows / 2f, 0, (float)mazeModel.Cols / 2f);
			SetCameraPosition(cameraOffset);
		}
		
		private void SetCameraPosition(Vector3 position)
		{
			view.SetCameraPosition(position);
		}
		
		private void OnDestroy()
		{
			SetCameraPosition(-cameraOffset);
			
			gameController.OnPause -= gameModel.Pause;
			gameController.OnResume -= gameModel.Resume;
		}
	}
}