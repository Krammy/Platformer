using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigimusGames
{
	[CreateAssetMenu(fileName = "NewCharacterProperties", menuName = "Character/Properties")]
	public class CharacterProperties : ScriptableObject
	{
		[Tooltip("The amount of acceleration per frame.")]
		public float Acceleration = 0.1f;
		[Tooltip("The max speed of the character.")]
		public float MaxSpeed = 5f;
		[Tooltip("The radius of the character.")]
		public float Radius = 0.5f;

		[Tooltip("The character's jump force.")]
		public float JumpForce = 5f;

		public float Gravity = -100f;
		public float GravityAcceleration = 0.1f;
	}
}


