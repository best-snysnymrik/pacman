using UnityEngine;
using System.Collections;

using Pacman.View.Scene;

namespace Pacman.Model
{
	public enum DialogType
	{
		Play,
		Pause
	}
	
	public class GuiManager
	{
		private static readonly GuiManager instance = new GuiManager();		
		
		static GuiManager() {}
		private GuiManager() {}
		
		public static GuiManager guiManager
		{
			get { return instance; }
		}
		
		private SceneView view;
		
		public void SetSceneView(SceneView view)
		{
			this.view = view;
		}
		
		public void ShowDialog(DialogType dialogType)
		{
			view.ShowDialog(dialogType);
			
			CheckActiveDialogsCounts();
		}
		
		public void CloseDialog(GameObject dialog)
		{
			view.CloseDialog(dialog);
			
			CheckActiveDialogsCounts();
		}
		
		private void CheckActiveDialogsCounts()
		{
			if (view.ActiveDialogsCount > 0)
				view.ShowDialogBack();
			else
				view.HideDialogBack();
		}
	}
}