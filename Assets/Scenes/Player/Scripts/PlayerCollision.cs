using UnityEngine;

// This script handles player collisions with other objects. It should be attached to the player

public class PlayerCollision : MonoBehaviour
{
    private PlayerController ControlScript; // Reference to the script that handles player input
	
	void Start()
	{
		ControlScript = GetComponent<PlayerController>();
	}

    // Called whenever a collision happens with another object
    /*void OnCollisionEnter2D(Collision2D CollisionInfo)
    {
		// TODO: This is a placeholder for an environmental object that harms the player
		// Similar can be done for when the knight's attack hitbox is collided with
        //if (CollisionInfo.collider.tag == "Obstacle")
        //{
        //    ControlScript.PlayerState = kDead; // Kill player
        //}
		
		if (CollisionInfo.collider.tag == "Floor")
		{
			ControlScript.SetFloorCollision(true);
		}
		else if (CollisionInfo.collider.tag == "Wall") // This is an else-if to prioritize jumping off the floor instead of a wall
		{
			ControlScript.SetWallCollision(true);
		}
    }*/

    // Called whenever two objects cease collision
    void OnCollisionExit2D(Collision2D CollisionInfo)
    {
        //ControlScript.SetFloorCollision(false);
		//ControlScript.SetWallCollision(false);
    }
}