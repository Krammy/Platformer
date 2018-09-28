using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigimusGames
{
	public class CharacterMove
	{
		#region Variables
		private Transform characterTransform;
		private CharacterInput characterInput;
		private CharacterProperties characterProperties;

		private Vector2 velocity;
		private bool touchingGround = false;
		#endregion

		#region Methods
		public CharacterMove(Transform characterTransform, CharacterInput characterInput, CharacterProperties characterProperties)
		{
			this.characterTransform = characterTransform;
			this.characterInput = characterInput;
			this.characterProperties = characterProperties;
		}

		private void SetMovementVelocity()
		{
			float targetSpeed = characterInput.Horizontal * characterProperties.MaxSpeed;
			// This is the target velocity we want to eventually reach.

			if (targetSpeed < velocity.x)
			{
				velocity.x = Mathf.Max(velocity.x - characterProperties.Acceleration, targetSpeed);
			}
			else if (targetSpeed > velocity.x)
			{
				velocity.x = Mathf.Min(velocity.x + characterProperties.Acceleration, targetSpeed);
			}
		}

		private void ApplyGravity()
		{
			float velocityOffset = characterProperties.GravityAcceleration * Time.deltaTime;
			float nextVelocity = Mathf.Max(velocity.y - velocityOffset, characterProperties.Gravity);

			Vector2 nextPosition = (Vector2)characterTransform.position + Vector2.up * nextVelocity * Time.deltaTime;

			if (nextVelocity < 0f)
			{
				// raycast next velocity step towards ground.
				// if it hits collider, set new position above surface and zero velocity.
				float dist = Vector2.Distance(characterTransform.position, nextPosition);

				RaycastHit2D hit = Physics2D.Raycast(characterTransform.position, Vector2.down, dist);
				if (hit.collider != null)
				{
					nextVelocity = 0f;
					characterTransform.position += Vector3.down * hit.distance;
					touchingGround = true;
				}
			}

			velocity.y = nextVelocity;
		}

		private void DrawDebugLine()
		{
			float velocityOffset = characterProperties.GravityAcceleration * Time.deltaTime;

			Vector2 startPos = characterTransform.position;
			Vector2 endPos = startPos + Vector2.down * velocityOffset;
			Debug.DrawLine(startPos, endPos, Color.blue);
		}

		private void CheckJump()
		{
			if (characterInput.Jump && touchingGround)
			{
				velocity.y = characterProperties.JumpForce;
			}
		}

		public void Tick()
		{
			SetMovementVelocity();
			
			ApplyGravity();
			CheckJump();

			if (velocity.y > 0f)
			{
				touchingGround = false;
			}

			characterTransform.position += (Vector3)velocity * Time.deltaTime;

			DrawDebugLine();
		}
		#endregion Methods
	}
}


