using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigimusGames
{
	public class Character : MonoBehaviour
	{
		#region Variables
		[SerializeField] private CharacterProperties characterProperties;

		private CharacterInput characterInput;
		private CharacterMove characterMove;

		private SpriteRenderer spriteRend;
		#endregion
		
		#region MonoBehaviour Methods
		private void Awake()
		{
			characterInput = new CharacterInput();
			characterMove = new CharacterMove(transform, characterInput, characterProperties);

			spriteRend = GetComponentInChildren<SpriteRenderer>();
		}
		
		private void Update ()
		{
			characterInput.Tick();
			if (characterInput.Jump)
			{
				spriteRend.color = Color.red;
			}
			else
			{
				spriteRend.color = Color.Lerp(spriteRend.color, Color.white, 0.1f);
			}
			characterMove.Tick();
		}
		#endregion
	}
}


