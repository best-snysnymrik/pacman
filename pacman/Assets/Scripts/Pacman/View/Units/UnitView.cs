using UnityEngine;
using System.Collections.Generic;

using Pacman.Model;
using Pacman.View.Units.Behavior;

namespace Pacman.View.Units
{
	public class UnitView : MonoBehaviour
	{
		public delegate void CompleteMoveHandler();
		public event CompleteMoveHandler OnCompleteMove;
		
		public delegate void CollisionDetectedHandler(string otherUnitId);
		public event CollisionDetectedHandler OnCollisionDetected;
		
		private Dictionary<Direction, float> angles = new Dictionary<Direction, float>()
		{
			{Direction.left, -90},
			{Direction.right, 90},
			{Direction.up, 0},
			{Direction.down, 180}
		};
		
		void Awake()
		{}
		
		void OnDestroy()
		{
			iTween.tweens.Clear();
		}
		
		public void SetMaterial(string materialName)
		{
			renderer.material = Resources.Load(materialName, typeof(Material)) as Material;
		}
		
		public void RotateTo(Direction direction)
		{
			iTween.RotateTo(gameObject, iTween.Hash(
                "y", angles[direction],
                "time", 0.1f));
		}
		
		public void MoveTo(Vector2 point, float speed)
		{
			Vector3 position = new Vector3(point.x, 0, point.y);
			
			iTween.MoveTo(gameObject, iTween.Hash(
                "position", position,
                "easetype", iTween.EaseType.linear,
                "speed", speed,
                "oncompletetarget", gameObject,
                "oncomplete", "CompleteMove"));
		}
		
		public void SetToPoint(Vector2 point)
		{
			transform.position = new Vector3(point.x, 0, point.y);
		}
		
		public void MoveToStart(Vector2 point, float speed)
		{
			var moveParameters = new MoveParameters(point, speed);
			
			if (transform.position.z != point.y)
			{
				if (transform.position.z < point.y)
					RotateTo(Direction.right);
				else
					RotateTo(Direction.left);
			}
							
			Vector3 position = new Vector3(transform.position.x, 0, point.y);
			
			iTween.MoveTo(gameObject, iTween.Hash(
                "position", position,
				"easetype", iTween.EaseType.linear,
                "speed", speed,
                "oncompletetarget", gameObject,
                "oncomplete", "CompleteMoveToStart",
				"oncompleteparams", moveParameters));
		}
		
		private void CompleteMoveToStart(object moveParameters)
		{			
			Vector2 point = ((MoveParameters)moveParameters).targetPoint;
			float speed = ((MoveParameters)moveParameters).speed;
			
			if (transform.position.x != point.x)
			{
				if (transform.position.x > point.x)
					RotateTo(Direction.up);
				else
					RotateTo(Direction.down);
			}			
				
			Vector3 position = new Vector3(point.x, 0, point.y);
			
			iTween.MoveTo(gameObject, iTween.Hash(
                "position", position,
				"easetype", iTween.EaseType.linear,
                "speed", speed,
                "oncompletetarget", gameObject,
                "oncomplete", "CompleteMove"));
		}
		
		private void CompleteMove()
		{
			OnCompleteMove();
		}
		
		public void Stop()
		{
			iTween.Stop(gameObject);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			OnCollisionDetected(other.gameObject.GetComponent<UnitMediator>().GetUnitId());
		}
	}
	
	public class MoveParameters
	{
		public Vector2 targetPoint;
		public float speed;
		
		public MoveParameters(Vector2 targetPoint, float speed)
		{
			this.targetPoint = targetPoint;
			this.speed = speed;
		}
	}
}