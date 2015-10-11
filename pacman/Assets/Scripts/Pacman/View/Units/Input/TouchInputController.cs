using UnityEngine;
using System.Collections;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Pacman.View.Units.Inputt
{
	public class TouchInputController : MonoBehaviour
	{
		private Vector2 swipeStartPosition = Vector2.zero;
		private bool isSwipe = false;
		private const float minSwipeDistance = 50.0f;
		PacmanModel model;
		
		public void SetModel(PacmanModel model)
		{
			this.model = model;
		}
		
		void FixedUpdate()
		{
			if (model == null)
				return;

			if (Input.touchCount == 0)
				return;
			
			foreach (Touch touch in Input.touches) 
			{
				switch (touch.phase) 
				{
				case TouchPhase.Began :
					isSwipe = true;
					swipeStartPosition = touch.position;
					break;
          
				case TouchPhase.Canceled :
					isSwipe = false;
					break;
          
				case TouchPhase.Ended :
					float swipeDistance = Vector2.Distance (touch.position, swipeStartPosition);
            
					if (isSwipe && swipeDistance >= minSwipeDistance) 
					{
						Vector2 direction = touch.position - swipeStartPosition;
						Vector2 move = Vector2.zero;
            
						if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) 
							move = Vector2.right * Mathf.Sign (direction.x);
						else 
							move = Vector2.up * Mathf.Sign (direction.y);
							
						if (move.x > 0)
							model.SetMoveDirection (Direction.right);
						else if (move.x < 0)
							model.SetMoveDirection (Direction.left);

						if (move.y > 0)
							model.SetMoveDirection (Direction.up);
						else if (move.y < 0)
							model.SetMoveDirection (Direction.down);
					}
					break;
				}
			}	
			
		}
	}
}