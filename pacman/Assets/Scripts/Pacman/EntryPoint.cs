using UnityEngine;
using System.Collections;

using Pacman.Data;
using Pacman.Model;

namespace Pacman
{
	public class EntryPoint : MonoBehaviour
	{		
		private GameData gameData = GameData.gameData;
		private GameController gameController = GameController.gameController;
		
		void Awake()
		{
			gameData.OnDataInitialized += DataInitialized;
			
			gameData.InitData(this);
		}
		
		void OnDestroy()
		{
			gameData.OnDataInitialized -= DataInitialized;
		}
		
		private void DataInitialized()
		{
			gameController.ShowMainMenu();
		}
	}
}