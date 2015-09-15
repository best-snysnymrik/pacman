using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using JsonFx.Json;

namespace Pacman.Logic
{
	public class GameLogic : MonoBehaviour
	{
		public static GameLogic logic;
		
		private List<MazeDef> levels = new List<MazeDef>();
		
		void Awake ()
		{
			if (logic == null)
			{
				DontDestroyOnLoad(gameObject);
				logic = this;
				
				Init();
			}
			else if (logic != this)
				Destroy(gameObject);
		}
		
		void Init()
		{
			InitDefs();
			InitState();
		}
		
		void InitDefs()
		{
			InitLevels();			
			//load bots and pacman defs
		}
		
		void InitLevels()
		{
			string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "levels.json");
			StartCoroutine(LinkStreamingFolder(filePath));
		}
		
		IEnumerator LinkStreamingFolder(string filePath)
		{
			string data;
			
			if (filePath.Contains("://"))
			{
				WWW linkstream = new WWW(filePath);
			
				while (!linkstream.isDone)
					yield return linkstream;
			
				data = linkstream.text;
			}
			else
			{
				var streamReader = new StreamReader(filePath);
        		data = streamReader.ReadToEnd();
        		streamReader.Close();
			}
			
			levels = JsonReader.Deserialize<Levels>(data).levels;
		}
		
		void InitState()
		{
		}
		
		public MazeDef GetCurrentMaze()
		{
			return levels[0];
		}
	}
}