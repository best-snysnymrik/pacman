using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using Pacman.Model;

namespace Pacman.View.Scene
{	
	public class SceneView : MonoBehaviour
	{
		[Serializable]
		private class SceneData
		{
			public GameCondition condition = GameCondition.MainMenu;
			public GameObject screenPrefab = null;
			public GameObject worldPrefab = null;
		};
		
		[Serializable]
		private class DialogData
		{
			public DialogType dialogType = DialogType.Play;
			public GameObject dialogPrefab = null;
		}
		
		[SerializeField]
		private GameObject Gui;
		[SerializeField]
		private CanvasScaler scaler;
		[SerializeField]
		private GameObject World;
		[SerializeField]
		private List<SceneData> scenes = new List<SceneData>();
		[SerializeField]
		private List<DialogData> dialogs = new List<DialogData>();
		[SerializeField]
		private GameObject DialogBack;
		
		private GameController gameController = GameController.gameController;
		private GuiManager guiManager = GuiManager.guiManager;
		
		private List<GameObject> activeDialogs = new List<GameObject>();
		private GameObject activeScreen;
		
		public int ActiveDialogsCount
		{
			get
			{
				return activeDialogs.Count;
			}			
		}
		
		void Awake()
		{
			gameController.SetSceneView(this);
			guiManager.SetSceneView(this);
			
			SetScale();
		}
						
		public void ShowScene(GameCondition condition)
		{
			DialogBack.SetActive(false);
			
			var sceneData = scenes.Find(x => x.condition == condition);
			
			activeScreen = InstantiateObject(sceneData.screenPrefab, Gui, false);
			InstantiateObject(sceneData.worldPrefab, World);			
		}
		
		public void ShowDialog(DialogType dialogType)
		{
			var dialogData = dialogs.Find(x => x.dialogType == dialogType);
			
			activeDialogs.Add(InstantiateObject(dialogData.dialogPrefab, Gui, false));
		}
		
		public void CloseDialog(GameObject obj)
		{
			if (activeDialogs.Contains(obj))
			{
				activeDialogs.Remove(obj);
				Destroy(obj);
			}
		}
		
		public void ShowDialogBack()
		{
			DialogBack.transform.SetSiblingIndex(activeDialogs.Count);
			DialogBack.SetActive(true);
		}
		
		public void HideDialogBack()
		{
			DialogBack.SetActive(false);
		}
		
		public void ClearScene()
		{			
			ClearGui();
			
			DestroyAllChildrens(World.transform);
		}
		
		private void ClearGui()
		{
			foreach (GameObject element in activeDialogs)
				Destroy(element);
			
			activeDialogs.Clear();
			
			Destroy(activeScreen);
		}
		
		private void DestroyAllChildrens(Transform tr)
		{
			foreach (Transform child in tr)
				Destroy(child.gameObject);
		}
		
		private GameObject InstantiateObject(GameObject prefab, GameObject parent, bool worldPositionStays = true)
		{
			if (prefab == null)
				return null;
			
			var instance = Instantiate(prefab) as GameObject;
			instance.transform.SetParent(parent.transform, worldPositionStays);
			
			return instance;
		}
		
		private void SetScale()
		{
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1024, 768);

            if (Screen.width == 0 || Screen.height == 0)
            {
                scaler.matchWidthOrHeight = 0.5f;
                return;
            }
			
            float referenceAspect = scaler.referenceResolution.x / scaler.referenceResolution.y;
            float screenAspect = (float) Screen.width / Screen.height;
            scaler.matchWidthOrHeight = screenAspect < referenceAspect ? 0 : 1;
		}
	}
}