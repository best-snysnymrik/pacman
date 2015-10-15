using UnityEngine;
using System.Collections;

using Pacman.Data;
using Pacman.Model;
using Pacman.Model.Unit;

namespace Pacman.View.Units
{
	public class UnitMediator : MonoBehaviour
	{
		[SerializeField]
		protected UnitView view;
		
		private GameData gameData = GameData.gameData;
		
		private UnitModel model;
		
		void Awake()
		{
			view.OnCompleteMove += Move;
			view.OnCollisionDetected += CollisionDetected;
		}
		
		void OnDestroy()
		{
			view.OnCompleteMove -= Move;
			view.OnCollisionDetected += CollisionDetected;
		}
		
		public void SetUnitModel(UnitModel model)
		{
			this.model = model;
			
			SetMaterial();
			
			if (model.UnitId != UnitDefId.Pacman)
				view.AddFrighteningBehavior();				
		}
		
		public string GetUnitId()
		{
			return model.UnitId;
		}
		
		public void SetFrighteningBehavior(float time)
		{
			view.SetFrighteningBehavior(time);
		}
		
		public void StopFrighteningBehavior()
		{
			view.StopFrighteningBehavior();
		}
		
		private void SetMaterial()
		{
			view.SetMaterial(gameData.defs.units[model.UnitId].material);
		}
		
		public void Move()
		{
			var nextMove = model.GetNextMovePoint();
			
			view.RotateTo(nextMove.direction);
			view.MoveTo(nextMove.point, model.Speed);
		}
		
		public void SetToPoint(Vector2 point)
		{
			view.SetToPoint(point);
		}
		
		public void MoveToStart(Vector3 position)
		{
			view.MoveToStart(position, model.Speed);
		}
		
		public void Stop()
		{
			view.Stop();
		}
		
		private void CollisionDetected(string otherUnitId)
		{
			if (model.UnitBehaviorMode == UnitBehaviorMode.normal)
			{
				// пакмана заловили
				if (model.UnitId == UnitDefId.Pacman)
					model.Catch();				
			}
			else
			{
				// режим "боязни", пакман заловил привидение
				if (otherUnitId == UnitDefId.Pacman)
					model.Catch();
			}
		}
	}
}