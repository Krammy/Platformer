using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DigimusGames
{
	public class CharacterMove
	{
		#region Variables
		private Transform characterTransform;
		private CharacterInput characterInput;
		private CharacterProperties characterProperties;

		private Rigidbody2D characterRb2d;

		private Vector2 velocity;
		private bool allowedToJump = false;
		#endregion

		#region Methods
		public CharacterMove(Transform characterTransform, CharacterInput characterInput, CharacterProperties characterProperties)
		{
			this.characterTransform = characterTransform;
			this.characterInput = characterInput;
			this.characterProperties = characterProperties;

			characterRb2d = characterTransform.GetComponent<Rigidbody2D>();
		}

		private void AddMovementVelocity()
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

		private void AddGravityVelocity()
		{
			float velocityOffset = characterProperties.GravityAcceleration * Time.deltaTime;
			float nextVelocity = Mathf.Max(velocity.y - velocityOffset, characterProperties.Gravity);

			//Vector2 nextPosition = (Vector2)characterTransform.position + Vector2.up * nextVelocity * Time.deltaTime;

			//if (nextVelocity < 0f)
			//{
			//	// raycast next velocity step towards ground.
			//	// if it hits collider, set new position above surface and zero velocity.
			//	//float dist = Vector2.Distance(characterTransform.position, nextPosition);

			//	RaycastHit2D hit = Physics2D.Raycast(characterTransform.position, Vector2.down, dist);
			//	if (hit.collider != null)
			//	{
			//		nextVelocity = 0f;
			//		characterTransform.position += Vector3.down * hit.distance;
			//		touchingGround = true;
			//	}
			//}

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
			if (characterInput.Jump && allowedToJump)
			{
				velocity.y = characterProperties.JumpForce;
				allowedToJump = false;
			}
		}

		private void DebugList<T>(List<T> myList)
		{
			Debug.Log("Outputting List:");
			for (int i = 0; i < myList.Count; i++)
			{
				Debug.Log(i + ": " + myList[i]);
			}
		}

		private void TryToMove()
		{
			Vector2 nextPosition = (Vector2)characterTransform.position + (velocity * Time.deltaTime);
			float dist = Vector2.Distance(characterTransform.position, nextPosition);

			Vector2 dir = (nextPosition - (Vector2)characterTransform.position).normalized;

			if (dir.y > 0f)
			{
				Debug.Log("Moving Direction: " + dir);
			}

			RaycastHit2D[] results = new RaycastHit2D[4];
			characterRb2d.Cast(dir, results, dist);

			List<RaycastHit2D> resultsList = results.ToList();
			
			// get result with smallest distance to collider.
			resultsList.Sort((a,b) => a.distance.CompareTo(b.distance));
			resultsList.RemoveAll(a => a.collider == null);

			if (resultsList.Count == 0 )
			{
				characterTransform.position = nextPosition;
				return;
			}
			
			// handle collision

			// if distance is zero, character won't move
			characterTransform.position += (Vector3)dir * resultsList[0].distance;

			if (dir.y < 0f)
			{
				velocity.y = 0f;
				allowedToJump = true;
			}
		}

		public void Tick()
		{
			AddMovementVelocity();
			AddGravityVelocity();
			
			CheckJump();

			TryToMove();
			// Debug.Log("Velocity: " + velocity);
		}
		#endregion Methods
	}
}


