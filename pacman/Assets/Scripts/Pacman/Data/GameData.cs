using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using JsonFx.Json;

namespace Pacman.Data
{
	public class GameData
	{
		public delegate void DataInitializedHandler();
		public event DataInitializedHandler OnDataInitialized;
				
		private static readonly GameData instance = new GameData();
				
		static GameData() {}
		private GameData() {}
		
		public static GameData gameData
		{
			get { return instance; }
		}
		
		public Defs defs { get; set; }
		public State state { get; set; }
		
		private DataLoader loader;
		
		private int initTasksCount;
		
		private const string defsFileName = "defs.json";
		private const string stateFileName = "state.json";
		private const string testStateFileName = "state_test.json";
		
		private string defsPath;
		private string statePath;
		
		public void Init(MonoBehaviour mono, bool isTest = false)
		{
			#if UNITY_EDITOR || UNITY_WEBPLAYER
			loader = new StreamDataLoader();
			#endif
			
			#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			loader = new WWWDataLoader(mono);
			#endif
						
			defsPath = System.IO.Path.Combine(Application.streamingAssetsPath, defsFileName);
			statePath = System.IO.Path.Combine(Application.persistentDataPath, stateFileName);
			
			if (isTest)
				statePath = System.IO.Path.Combine(Application.streamingAssetsPath, testStateFileName);
		}
		
		public void LoadData()
		{
			initTasksCount = 2;
			
			loader.LoadDefsData(defsPath);
			
			if (!System.IO.File.Exists(statePath))
			{
				System.IO.File.Create(statePath);
				loader.LoadStateData(System.IO.Path.Combine(Application.streamingAssetsPath, stateFileName));
			}
			else
			{
				if (loader is StreamDataLoader)
					loader.LoadStateData(statePath);
				else
				{
					StreamDataLoader streamDataLoader = new StreamDataLoader();
					streamDataLoader.LoadStateData(statePath);
				}
			}
		}
		
		public void SetDefsData(string data)
		{
			defs = JsonReader.Deserialize<Defs>(data);
			CompleteInit();
		}
		
		public void SetStateData(string data)
		{
			state = JsonReader.Deserialize<State>(data);
			CompleteInit();
		}
		
		private void CompleteInit()
		{
			initTasksCount--;
			
			if (initTasksCount == 0)
				OnDataInitialized();
		}
		
		public void CreateNewMaze(string mazeId)
		{
			Maze newMaze = new Maze();
			
			newMaze.level = 0;
			newMaze.scores = 0;
			newMaze.dots = 0;
			newMaze.lives = defs.commonValues.livesCount;
			
			newMaze.elements = new List<int>(defs.mazes[mazeId].view.elements);
			
			newMaze.units = new Dictionary<string, UnitPoint>();
			var unitsDef = defs.mazes[mazeId].units;
			
			foreach (var unitDef in unitsDef)
			{
				UnitPoint unitPoint = new UnitPoint();
				unitPoint.direction = unitDef.Value.direction;
				unitPoint.position = unitDef.Value.position;
				
				newMaze.units[unitDef.Key] = unitPoint;
			}
			
			state.mazes[mazeId] = newMaze;
			
			SaveStateData();
		}
		
		public void SaveStateData()
		{
			loader.SaveStateData(statePath, JsonWriter.Serialize(state));
		}
	}
	
	
	public class DataLoader
	{
		protected GameData gameData = GameData.gameData;
		
		public virtual void LoadDefsData(string filePath){}
		public virtual void LoadStateData(string filePath){}
		
		public virtual void SaveStateData(string filePath, string state)
		{
			var streamWriter = new StreamWriter(filePath);
			streamWriter.Write(state);
			streamWriter.Close();
		}
	}
	
	public class WWWDataLoader: DataLoader
	{
		private MonoBehaviour mono;
		
		public WWWDataLoader(MonoBehaviour mono)
		{
			this.mono = mono;
		}
		
		public override void LoadDefsData(string filePath)
		{
			mono.StartCoroutine(LoadDefs(filePath));
		}
		
		public override void LoadStateData(string filePath)
		{
			mono.StartCoroutine(LoadState(filePath));
		}
		
		private IEnumerator LoadDefs(string filePath)
		{
			WWW linkstream = new WWW(filePath);			
			yield return linkstream;
			
			gameData.SetDefsData(linkstream.text);
		}
		
		private IEnumerator LoadState(string filePath)
		{
			WWW linkstream = new WWW(filePath);
			yield return linkstream;
			
			gameData.SetStateData(linkstream.text);
		}
	}
	
	public class StreamDataLoader: DataLoader
	{
		public StreamDataLoader()
		{
		}
		
		public override void LoadDefsData(string filePath)
		{
			var streamReader = new StreamReader(filePath);
        	gameData.SetDefsData(streamReader.ReadToEnd());
        	streamReader.Close();
		}
		
		public override void LoadStateData(string filePath)
		{
			var streamReader = new StreamReader(filePath);
			gameData.SetStateData(streamReader.ReadToEnd());
        	streamReader.Close();
		}
	}
}