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
		
		private int initTasksCount;
		
		public void InitData(MonoBehaviour mono, bool isTest = false)
		{
			string defsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "defs.json");
			string statePath = System.IO.Path.Combine(Application.streamingAssetsPath, "state.json");
			
			if (isTest)
				statePath = System.IO.Path.Combine(Application.streamingAssetsPath, "state_test.json");
				
			
			DataLoader loader;
			
			#if UNITY_EDITOR || UNITY_WEBPLAYER
			loader = new StreamDataLoader();
			#endif
			
			#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
			loader = new WWWDataLoader(mono);
			#endif
			
			initTasksCount = 2;
			
			loader.LoadDefsData(defsPath);
			loader.LoadStateData(statePath);
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
	}
	
	
	public class DataLoader
	{
		protected GameData gameData = GameData.gameData;
		
		public virtual void LoadDefsData(string filePath){}
		public virtual void LoadStateData(string filePath){}
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