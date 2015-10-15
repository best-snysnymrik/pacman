using UnityEngine;
using System.Collections;

using Pacman.Data;
using Pacman.Model.Unit;

namespace Pacman.View.Units
{
	public class UnitMediator : MonoBehaviour
	{
		[SerializeField]
		protected UnitView view;
		
		protected GameData gameData = GameData.gameData;
		
		protected UnitModel model;
		
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
		}
		
		public string GetUnitId()
		{
			return model.UnitId;
		}
		
		protected virtual void SetMaterial()
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

		protected virtual void CollisionDetected(string otherUnitId)
		{}
	}
}