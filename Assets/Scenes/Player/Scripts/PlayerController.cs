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
    [SerializeField] private float MoveScale = 50.0f; 	// Movement speed left/right
	[SerializeField] private float FallScale = 0.5f; 	// Fall speed modifier
    [SerializeField] private float FloorJumpForce = 10.0f; 	// Default state - Multiplier when jumping off floor
	[SerializeField] private float WallJumpForce = 8.0f; 	// Default state - Multiplier when jumping off wall
	[SerializeField] private float FastMoveMultiplier = 2.0f; 	// Fast movement state 
	[SerializeField] private float FastJumpMultiplier = 0.25f; 	// Fast movement state
	[SerializeField] private float JumpMoveMultiplier = 0.5f; 	// Better jump state
	[SerializeField] private float JumpJumpMultiplier = 5.0f; 	// Better jump state
	[SerializeField] private Animator anim; // reference to animator
	
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
		PlayerRB.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent player from rotating, set this to allow rotation when dead b/c it's funny
	}
	
    void FixedUpdate()
    {
		// Get input
		horizontalInput = Input.GetAxisRaw("Horizontal");
		isTryingToJump = Input.GetButton("Jump");
		
		// This value initally starts off as the horizontal input and is then modifed by player state
		float NewHorizontalVelocity = horizontalInput * MoveScale;// ( Time.fixedDeltaTime;
		
		// Also modified by state, initially based on if player is touching floor, wall, or neither (set to zero to nullify modifications)
		float NewVerticalVelocity = (isTouchingFloor) ? FloorJumpForce : ((isTouchingWall) ? WallJumpForce : 0.0f);
		
		// Modify values based on state
		switch (PlayerState)
		{
			case (eState.kDefault):
			{
				//NewHorizontalVelocity *= MoveForce; // no
				//for now have this here
				if(horizontalInput == 0f){
					//idle state animation 
					anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
				}
				else if(horizontalInput != 0f){
					//animation for moving
					anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
				}
			}
			break;
			
			case (eState.kFast):
			{
				NewHorizontalVelocity *= FastMoveMultiplier;
				NewVerticalVelocity *= FastJumpMultiplier;
			}
			break;
			
			case (eState.kJump):
			{
				NewHorizontalVelocity *= JumpMoveMultiplier;
				NewVerticalVelocity *= JumpJumpMultiplier;
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
				NewHorizontalVelocity = 0.0f;
				NewVerticalVelocity = 0.0f;
			}
			break;
			
			// These should never occur, they are here for good practice
			case (eState.kNumStates): { PlayerState = eState.kDead; } break;
			default: { PlayerState = eState.kDead; } break;
		}
		
		PlayerRB.AddForce(new Vector2(NewHorizontalVelocity, 0)); // Apply horizontal movement (using AddForce to make player need to accelerate)
		
		// Apply vertical movement
		if (isTryingToJump) // If player tries to jump
		{
			if (isTouchingFloor || (isTouchingWall && !recentlyJumped)) // If player is on a surface, apply velocity immediately
			{
				PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, NewVerticalVelocity);
				recentlyJumped = true; // Prevents rapidly jumping off the same wall (player must release jump and press again)
			}
		}
		else if (PlayerRB.velocity.y > 0) // Else, player has released jump button, if they have residual upward velocity then release was mid-jump
		{
			PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, PlayerRB.velocity.y * FallScale); // Stop upward velocity (stop the jump)
		}
		else
		{
			recentlyJumped = false; // Allows another wall jump
		}
		
		// Determine whether to horizontally flip the player based on directional input
		if ((isFacingRight && (NewHorizontalVelocity < 0.0f)) || (!isFacingRight && (NewHorizontalVelocity > 0.0f)))
		{
			isFacingRight = !isFacingRight;
			
			// Scale values cannot be modified directly for some reason
			Vector3 NewScale = transform.localScale;
			NewScale.x *= -1.0f;
			transform.localScale = NewScale;
		}
    }
	
	// Functions called by the PlayerCollision script, used as setters for surface collision so they don't have to be public
	public void SetFloorCollision(bool isCollidingFloor)
	{
		isTouchingFloor = isCollidingFloor;
	}
	public void SetWallCollision(bool isCollidingWall)
	{
		isTouchingWall = isCollidingWall;
	}
	// I resent having to add boilerplate code like this when the bools could just be public, but this is considered good practice
}
