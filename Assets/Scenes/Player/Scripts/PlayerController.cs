using UnityEngine;

// This script handles user input for moving the player, and the player's internal state. It should be attached to the player
// Note: I am following the general guidelines from this: https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity
// I am going with the convention that externally accessable variables are PascalCase, and private are camelCase

public class PlayerController : MonoBehaviour
{
	// A decent amount of help came from this tutorial: https://www.youtube.com/watch?v=K1xZ-rycYY8
	
	// Player state declaration
	public enum eState : int
	{
		kDefault, 	// Regular movement and jump
		kFast, 		// Fast movement, bad jump
		kJump, 		// Slow movement, high jump
		kGodmode, 	// Invulnerable
		kDead, 		// Game over
		kNumStates
	}
	
	private Rigidbody2D PlayerRB; // Reference to the player's physics

	// Tunable values
	[SerializeField] private float MoveScale = 1000.0f; 	// Movement speed left/right
	[SerializeField] private float SlowDownScale = 2.0f; 	// How fast the player slows down
	[SerializeField] private float FloorJumpForce = 25.0f; 	// Universal value for jumping off floor
	[SerializeField] private float WallJumpForce = 20.0f; 	// Universal value forjumping off wall
	[SerializeField] private float FastMoveMultiplier = 2.0f; 	// Fast movement state 
	[SerializeField] private float FastJumpMultiplier = 0.25f; 	// Fast movement state
	[SerializeField] private float JumpMoveMultiplier = 0.5f; 	// Better jump state
	[SerializeField] private float JumpJumpMultiplier = 5.0f; 	// Better jump state
	
	// These values change according to player input/movement
	public eState PlayerState = eState.kDefault;
	private float horizontalInput = 0.0f; 	// Left/Right input
	private bool isTryingToJump = false; 	// Jump input
	private bool recentlyJumped = false; 	// True the moment after the player jumps, False the moment they begin falling back down
	private bool isFacingRight = true;  	// The player can only face left/right, false=left and the player is horizontally flipped
	private bool isTouchingFloor = false; 	// True when colliding with any floor
	private bool isTouchingWall = false; 	// True when colliding with any wall
	
	void Start()
	{
		PlayerRB = GetComponent<Rigidbody2D>(); // Attach this script to the rigidbody of the game object it is being used on
		PlayerRB.constraints = RigidbodyConstraints2D.FreezeRotation; // Ensure player cannot rotate
		// On death maybe we can disable this constraint because they way it falls over is funny
	}
	
	// Physics updates
	void FixedUpdate()
	{
		// Get input
		horizontalInput = Input.GetAxisRaw("Horizontal");
		isTryingToJump = Input.GetButton("Jump");
		
		// This value initally starts off as the horizontal input and is then modifed by player state
		float targetXVelocity = horizontalInput * MoveScale * Time.fixedDeltaTime;
		
		// Also modified by state, initially based on if player is touching floor, wall, or neither (set to zero to nullify modifications)
		float targetYVelocity = ((isTouchingFloor) ? FloorJumpForce : ((isTouchingWall) ? WallJumpForce : 0.0f));
		
		// Modify values based on state
		switch (PlayerState)
		{
			case (eState.kDefault):
			{
				//newHorizontalVelocity *= MoveForce; // not yet
			}
			break;
			
			case (eState.kFast):
			{
				targetXVelocity *= FastMoveMultiplier;
				targetYVelocity *= FastJumpMultiplier;
			}
			break;
			
			case (eState.kJump):
			{
				targetXVelocity *= JumpMoveMultiplier;
				targetYVelocity *= JumpJumpMultiplier;
			}
			break;
			
			case (eState.kGodmode):
			{
				// yeah
			}
			break;
			
			case (eState.kDead):
			{
				// Zero out all user movement attempts
				targetXVelocity = 0.0f;
				targetYVelocity = 0.0f;
			}
			break;
			
			// These should never occur, they are here for good practice
			case (eState.kNumStates): { PlayerState = eState.kDead; } break;
			default: { PlayerState = eState.kDead; } break;
		}
		
		PlayerRB.AddForce(new Vector2(targetXVelocity, 0)); // Apply horizontal movement (using AddForce to make player need to accelerate)
		
		// Apply vertical movement
		if (isTryingToJump) // If player tries to jump
		{
			if (isTouchingFloor) // If player is on a surface, find out what type what apply force immediately
			{
				PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, targetYVelocity);
				recentlyJumped = true; // Prevents rapidly jumping off the same wall (player must release jump and press again)
			}
			else if (isTouchingWall && !recentlyJumped)
			{
				PlayerRB.velocity = new Vector2((targetYVelocity *((isFacingRight) ? -1 : 1)), targetYVelocity);
				Flip(); // Turn player around
				recentlyJumped = true; // Prevents rapidly jumping off the same wall (player must release jump and press again)
				
				// Due to the way this interacts with the Flip() function, this has the effect of if the player manages to face backwards
				// on a wall (such as sliding into it due to inertia), the wall jump, while going up, makes the player face into the wall
				// and does not push away from the wall, resuming normal behavior on the next jump. Although this wasn't the originally
				// intended behavior of the wall jump, I think it is a decent game "mechanic", it allows the player to catch themselves
			}
		}
		else if (PlayerRB.velocity.y > 0) // Else, player has released jump button, if they have residual upward velocity then release was mid-jump
		{
			PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, PlayerRB.velocity.y * 0.5f); // Stop upward velocity (stop the jump)
		}
		else
		{
			recentlyJumped = false; // Allows another wall jump
		}
		
		// If the player isn't attempting to move, apply slowdown on x-axis
		if (horizontalInput == 0.0f)
		{
			PlayerRB.AddForce(new Vector2(-PlayerRB.velocity.x * SlowDownScale, 0));
		}
		
		// Determine whether to horizontally flip the player based on directional input
		if ((isFacingRight && (targetXVelocity < 0.0f)) || (!isFacingRight && (targetXVelocity > 0.0f)))
		{
			Flip();
		}
	}
	
	// Flip player about horizontal axis, returns new value if it is ever needed
	private bool Flip()
	{
		isFacingRight = !isFacingRight;
		
		// Scale values cannot be modified directly for some reason
		Vector3 NewScale = transform.localScale;
		NewScale.x *= -1.0f;
		transform.localScale = NewScale;
		
		return isFacingRight;
	}
	
	// Functions called by the PlayerCollision script, used as setters for surface collision so they don't have to be public
	public void SetFloorCollision(bool IsCollidingFloor)
	{
		isTouchingFloor = IsCollidingFloor;
	}
	public void SetWallCollision(bool IsCollidingWall)
	{
		isTouchingWall = IsCollidingWall;
	}
	// I resent having to add boilerplate code like this when the bools could just be public, but this is considered good practice
}
