using UnityEngine;
using System.Collections;

using Pacman.Model;
using Pacman.Model.Unit;

namespace Pacman.View.Units.Inputt
{
	public class KeyboardInputController : MonoBehaviour
	{
		PacmanModel model;
		
		public void SetModel(PacmanModel model)
		{
			this.model = model;
		}
		
		void FixedUpdate()
		{
			if (model == null)
				return;
			
			float horizontalMove = Input.GetAxis("Horizontal");
			float verticalMove = Input.GetAxis("Vertical");		
			
			if (verticalMove > 0)
				model.SetMoveDirection(Direction.up);
			else if (verticalMove < 0)
				model.SetMoveDirection(Direction.down);
			
			if (horizontalMove > 0)
				model.SetMoveDirection(Direction.right);
			else if (horizontalMove < 0)
				model.SetMoveDirection(Direction.left);
		}
	}
}