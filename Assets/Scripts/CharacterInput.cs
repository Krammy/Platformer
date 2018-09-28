using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigimusGames
{
	public class CharacterInput
	{
		#region Variables
		public float Horizontal = 0f;
		public bool Jump = false;
		#endregion

		public void Tick()
		{
			Horizontal = Input.GetAxis("Horizontal");
			Jump = Input.GetButtonDown("Jump");
		}
	}
}


