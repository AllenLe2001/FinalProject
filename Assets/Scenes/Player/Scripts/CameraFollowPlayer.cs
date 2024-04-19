using UnityEngine;

// This script handles moving the main camera to the player. It should be attached to the main camera

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField] private Transform player; 	// Reference to the player
	[SerializeField] private Vector3 offset; 	// Should be (0, 0, -10) by default

	// Move the camera to the player, offset by a set amount
	void FixedUpdate()
	{
		transform.position = player.position + offset;
	}
}
