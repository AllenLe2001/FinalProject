using System.Collections;
using UnityEngine;

// This script handles user input for moving the player, and the player's internal state. It should be attached to the player
// Note: I am following the general guidelines from this: https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity
// I am going with the convention that externally accessable variables are PascalCase, and private are camelCase

public class PlayerController : MonoBehaviour
{
	// A decent amount of help came from these tutorials: youtu.be/K1xZ-rycYY8 youtu.be/KKGdDBFcu0Q
	public Animator animator;
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
	
	// Tunable values
	[SerializeField] private float MaxRunningSpeed = 15f; 	// Target speed we want the player to reach, and be limited by
	[SerializeField] private float RunAcceleration = 5f; 	// Rate at which it takes to accelerate from 0 to MaxRunningSpeed
	[SerializeField] private float RunDeceleration = 5f; 	// Same, but from MaxRunningSpeed to 0.
	
	[SerializeField] private float FallingGravityMult = 1.5f; 	// Multiplier of _gravityScale when falling
	[SerializeField] private float TerminalVelocity = 30f; 		// Maximum speed the player is made to fall
	
	[Range(0f, 1)] [SerializeField] private float AccelerationMidair = 0.35f; // Multipliers applied to acceleration when midair
	[Range(0f, 1)] [SerializeField] private float DecelerationMidair = 0.65f;
	
	[SerializeField] private bool ConserveMomentum = false; 	// When true, this allows the player to accelerate slightly above MaxRunningSpeed
	[SerializeField] private bool DampenMidairVelocity = true; 	// When false, this allows the player to move midair
	
	[SerializeField] private float JumpHeight = 4f; 	// Height achieved from jumping off the floor
	[SerializeField] private float JumpDuration = 0.5f; // Duration between applying the force of the jump and when upwards velocity reaches 0
	
	[SerializeField] private float JumpCancelGravityMult = 2f; 				// Multiplier of _gravityScale for if player releases jump input mid jump
	[Range(0f, 1)] [SerializeField] private float JumpApexGravityMult = 0f; // Multiplier to reduce gravity while close to the apex of a jump
	
	[SerializeField] private float JumpApexThreshold = 0f; 			// Adjustable value to allow for increased "floatiness" around the apex of a jump
	[SerializeField] private float JumpApexAccelerationMult = 0f; 	// Multiplier to acceleration to reduce it when at the apex of a jump
	[SerializeField] private float JumpApexMaxSpeedMult = 0.5f; 	// Multiplier to target velocity to reduce effective max speed when at the apex of a jump
	
	[SerializeField] private Vector2 WallJumpForce = new Vector2(6f, 8f); 	// Force applied by a wall jump
	[Range(0f, 1f)] [SerializeField] private float WallJumpPenalty = 0f; 	// Value used by a lerp function to reduce player movement during a wall jump
	[Range(0f, 1.5f)] [SerializeField] private float WallJumpDuration; 		// Duration of a wall jump
	[SerializeField] private bool TurnOnWallJump = true; 					// When true, Flip() is called upon a wall jump
	
	[Range(0.01f, 0.5f)] [SerializeField] private float MoveBufferThreshold = 0.01f; 	// Small limit value used when checking for low velocity in FixedUpdate
	[Range(0.01f, 0.5f)] [SerializeField] private float JumpBufferDuration = 0.1f; 		// Used in comparisons for jump checks 
	
	// FIXME: Re-implement these in their original usage
	[SerializeField] private float FastMoveMultiplier = 2.0f; // Fast movement state 
	//[SerializeField] private float FastJumpMultiplier = 0.25f;
	[SerializeField] private float JumpMoveMultiplier = 0.5f; // Better jump state
	//[SerializeField] private float JumpJumpMultiplier = 5.0f;
	
	// Physics-related references
    public Rigidbody2D PlayerRB { get; private set; } 		// Player's overall physics, publicly viewable but not writable
	[SerializeField] private LayerMask _environmentLayer; 	// The assigned layer of the environment (floor/walls)
	[SerializeField] private string _floorTag = "Floor"; 	// The assigned tag of the floors
	[SerializeField] private string _wallTag = "Wall"; 		// The assigned tag of the walls
	
	[SerializeField] private Transform _floorCheckPoint; // Transform that represents a point which acts as the "feet" of the player
	[SerializeField] private Vector2 _floorCheckSize = new Vector2(0.49f, 0.03f); // Size of hitbox centered on the point above
	
	[SerializeField] private Transform _wallCheckPoint; // Transform that represents a point which acts as the "hands" of the player
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f); // Size of hitbox centered on the point above'
	
	// These values change according to player input/movement/location
	public eState PlayerState { get; private set; } = eState.kDefault; // Self explanatory variable name
	private float _gravityScale = 0f; 	// Multiplier of gravity, stored in a variable so it can be easily read and modified
	private float _horizonalInput = 0f; // Left/Right input
	
	public bool IsFacingRight { get; private set; } = true; 	// The player can only face left/right, false=left and the player is horizontally flipped
	public bool IsJumping { get; private set; } = false; 		// True during inital part of jump from a floor
	public bool IsWallJumping { get; private set; } = false; 	// Same, but from a wall
	
	public float LastTimeOnFloor { get; private set; } = 0f; 	// Stores the last moment that the player was in contact with the floor
	public float LastTimeOnWall { get; private set; } = 0f; 	// Same, but for walls
	public float LastTimeJumpDown { get; private set; } = 0f; 	// Stores the last time the jump button was pressed down
	
	private float _wallJumpStartTime = 0f; 	// The time that a wall jump was initiated
	private bool _cancelledJump = false; 	// True when player released jump input while there was still some residual upwards velocity
	private bool _fallingFromJump = false; 	// True during the brief period of falling after a jump
	
	// Initialization
	private void Start()
	{
		PlayerRB = GetComponent<Rigidbody2D>();
		
		// Calculate the player's gravity scale using the formula (gravity = 2 * JumpHeight / JumpDuration^2) 
		_gravityScale = ((2 * JumpHeight) / (JumpDuration * JumpDuration)) / -Physics2D.gravity.y;
		
		PlayerRB.gravityScale = _gravityScale;
		IsFacingRight = true;
	}
	
	// Input updates
	private void Update()
	{
		animator.SetFloat("Speed", Mathf.Abs(_horizonalInput));
		// Subtract these timers, they are immediately reset upon if the right conditions are met
        LastTimeOnFloor -= Time.deltaTime;
		LastTimeOnWall -= Time.deltaTime;
		LastTimeJumpDown -= Time.deltaTime;
		
		// Get horizontal input
		_horizonalInput = Input.GetAxisRaw("Horizontal");
		
		// Get jump input, based on if this is the exact frame that the player has pressed/released the button
		if(Input.GetButtonDown("Jump")) // Pressed
        {
			LastTimeJumpDown = JumpBufferDuration;
        }
		if (Input.GetButtonUp("Jump") && (PlayerRB.velocity.y > 0) && (IsJumping || IsWallJumping)) // Released
		{
			_cancelledJump = true; // The latter part of the condition is checking if the player is mid-jump, essentially asking if we can "cancel" the jump
		}
    }
	
	// Physics updates
    private void FixedUpdate()
	{
		// Determine if the player model should be flipped about x-axis based on horizontal input
		if ((IsFacingRight && (_horizonalInput < 0.0f)) || (!IsFacingRight && (_horizonalInput > 0.0f)))
		{
			Flip();
		}
		
		// Collision checks
		Collider2D FloorCollision = Physics2D.OverlapBox(_floorCheckPoint.position, _floorCheckSize, 0, _environmentLayer);
		Collider2D FrontCollision = Physics2D.OverlapBox(_wallCheckPoint.position, _wallCheckSize, 0, _environmentLayer);
		
		// If there is contact with a floor/wall
		// These if-statements take advantage of the fact that if there was no detected collision, they are nullptr and evaluated as false
		if (!IsJumping && FloorCollision && (FloorCollision.CompareTag(_floorTag)))
		{
			LastTimeOnFloor = JumpBufferDuration; // This variable being greater than 0 acts as a "IsOnFloor = true" check
		}
		if (!IsWallJumping && FrontCollision && (FrontCollision.CompareTag(_wallTag)))
		{
			LastTimeOnWall = JumpBufferDuration; // Same, but for walls
		}
		
		// If player has reached the apex of a jump off a floor
		if (IsJumping && (PlayerRB.velocity.y < 0))
		{
			IsJumping = false; // This is considered no longer jumping
			
			if(!IsWallJumping) // If the player has NOT initiated a wall jump in the middle of a floor jump
			{
				_fallingFromJump = true; // The player is now considered to be falling from a jump
			}
		}
		
		// If player has jumped off a wall and the maximum length of time for this has passed
		if (IsWallJumping && ((Time.time - _wallJumpStartTime) > WallJumpDuration))
		{
			IsWallJumping = false; // This is considered no longer wall jumping
		}
		
		// If the player is on the floor, not jumping at all, reset these booleans
		if ((LastTimeOnFloor > 0) && !IsJumping && !IsWallJumping)
        {
			_cancelledJump = false;
			_fallingFromJump = false;
		}
		
		// Handle jumps
		if (!IsJumping && (LastTimeOnFloor > 0) && (LastTimeJumpDown > -JumpBufferDuration)) // Floor jump
		{
			// If player isn't jumping, they are on the floor, and have recently pressed jump
			IsJumping = true;
			IsWallJumping = false;
			_cancelledJump = false;
			_fallingFromJump = false;
			LastTimeJumpDown = 0; // Prevents jumping multiple times from one press
			LastTimeOnFloor = 0;
			
			// Add upwards velocity, compensating for negative y velocity If the player is falling, so force is always the same
			// That is just to cover the edge case of a jump starting on the same frame that the player lands on the ground
			Vector2 NewForce = Vector2.up * Mathf.Abs(_gravityScale * Physics2D.gravity.y) * JumpDuration;
			
			if (PlayerRB.velocity.y < 0) // Compensate for falling if necessary
			{
				NewForce.y -= PlayerRB.velocity.y;
			}
			PlayerRB.AddForce(NewForce, ForceMode2D.Impulse);
		}
		else if (!IsWallJumping && (LastTimeOnWall > 0) && (LastTimeJumpDown > -JumpBufferDuration)) // Wall jump
		{
			// Else, If player isn't wall jumping, they are on the wall, and have recently pressed jump
			// This is an else-if to prioritize the floor jump
			IsWallJumping = true;
			IsJumping = false;
			_cancelledJump = false;
			_fallingFromJump = false;
			_wallJumpStartTime = Time.time;
			LastTimeJumpDown = 0; // Prevents wall jumping multiple times from one press
			LastTimeOnFloor = 0;
			LastTimeOnWall = 0;
			
			// Add upwards velocity, as well as horizontal velocity in the opposite direction that the player is facing (away from wall)
			// Just like floor jump, also compensate for If the player is moving or falling
			Vector2 NewForce = new Vector2(WallJumpForce.x * ((IsFacingRight) ? -1 : 1), WallJumpForce.y);
			
			if (Mathf.Sign(PlayerRB.velocity.x) != Mathf.Sign(NewForce.x)) // Compensate for moving in opposite direction
			{
				NewForce.x -= PlayerRB.velocity.x;
			}
			if (PlayerRB.velocity.y < 0) // Compensate for falling
			{
				NewForce.y -= PlayerRB.velocity.y;
			}
			PlayerRB.AddForce(NewForce, ForceMode2D.Impulse);
			
			if (TurnOnWallJump)
			{
				Flip();
			}
		}
		
		// Apply higher gravity if player released jump or is falling
		PlayerRB.gravityScale = _gravityScale; // Set to default value
		if (_cancelledJump) // Jump button released
		{
			PlayerRB.gravityScale *= JumpCancelGravityMult;
			
			// Prevent insanely high fall speeds
			PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Mathf.Max(PlayerRB.velocity.y, -TerminalVelocity));
		}
		else if ((IsJumping || IsWallJumping || _fallingFromJump) && (Mathf.Abs(PlayerRB.velocity.y) < JumpApexThreshold)) // Mid-jump
		{
			PlayerRB.gravityScale *= JumpApexGravityMult;
		}
		else if (PlayerRB.velocity.y < 0) // Falling
		{
			PlayerRB.gravityScale *= FallingGravityMult; // Higher gravity for more satisfying falling
			
			// Prevent insanely high fall speeds
			PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, Mathf.Max(PlayerRB.velocity.y, -TerminalVelocity));
		}
		// If none of the above conditions are true, then default gravity is used as it was not changed
		
		// Calculate velocity based on input and the maximum speed, lerped based on direction and speed
		// The ternary statement acts as the lerp amount, which reduces the velocity if the player is wall jumping
		float targetXVelocity = Mathf.Lerp(PlayerRB.velocity.x, (_horizonalInput * MaxRunningSpeed), ((IsWallJumping) ? WallJumpPenalty : 1));
		
		bool isMoving = Mathf.Abs(targetXVelocity) > MoveBufferThreshold;
		// Calculate acceleration based on if the player is trying to accelerate (continuing to move in the same direction)
		// or trying to decelerate (move in other direction or stop moving entirely). The LastTimeOnFloor variable lets us
		// know if the player is in midair, so the value is then also multiplied by the midair acceleration modifier.
		float accelerationRate = (isMoving) ? ProperAcceleration(RunAcceleration) : ProperAcceleration(RunDeceleration);
		if (LastTimeOnFloor <= 0)
		{
			accelerationRate *= (isMoving) ? AccelerationMidair : DecelerationMidair;
		}
		
		// Modify values based on state, one of the oldest remnants of earlier versions of this script, currently doesn't affect jumps
		// Note that this does not result in an exponential increase, but instead applies once, accelerationRate is reset each frame
		switch (PlayerState)
		{
			case (eState.kDefault):
			{
				// Currently, the default state does not apply any modifiers
			}
			break;
			
			case (eState.kFast):
			{
				accelerationRate *= FastMoveMultiplier;
			}
			break;
			
			case (eState.kJump):
			{
				accelerationRate *= JumpMoveMultiplier;
			}
			break;
			
			case (eState.kGodmode):
			{
				// Currently, godmode does not apply any modifiers
			}
			break;
			
			case (eState.kDead):
			{
				// Zero out all user movement attempts
				accelerationRate = 0.0f;
			}
			break;
			
			// These should never occur, these cases are covered for good practice, just instantly change state to dead
			case (eState.kNumStates): { PlayerState = eState.kDead; } break;
			default: { PlayerState = eState.kDead; } break;
		}
		
		// Increase acceleration and velocity at the apex of the jump, results in jumping feeling more responsive and natural
		if ((IsJumping || IsWallJumping || _fallingFromJump) && (Mathf.Abs(PlayerRB.velocity.y) < JumpApexThreshold))
		{
			accelerationRate *= JumpApexAccelerationMult;
			targetXVelocity *= JumpApexMaxSpeedMult;
		}
		
		// If we are in "conserve momentum" mode, and the player is in midair trying to move
		if(isMoving && ConserveMomentum && (LastTimeOnFloor < 0))
		{
			// If the player is moving in their desired direction (they are not attempting to move in the opposite
			// direction that they are currently moving), but they are moving faster than the maximum allowed speed
			// This is declared as a variable in accordance with the professor's best practice advice
			bool isTooFast = (Mathf.Sign(PlayerRB.velocity.x) == Mathf.Sign(targetXVelocity)) && (Mathf.Abs(PlayerRB.velocity.x) > Mathf.Abs(targetXVelocity));
			
			if (isTooFast)
			{
				// Set accelerationRate to 0, the result of this is that the player is no longer being capped by the max speed
				// Under normal circumstances (ConserveSpeed is false) this script otherwise tries to decelerate the player
				accelerationRate = 0;
			}
		}
		
		// This is the key part of smoother movement, we must make the velocity based on how close the player already
		// is moving to the maximum velocity. First get the difference between these two values, then apply along x-axis
		// An additional improvement is NOT multiplying by Time.fixedDeltaTime, as .AddForce already handles this
		float targetXMovement = (targetXVelocity - PlayerRB.velocity.x) * accelerationRate;
		
		// Convert to vector and apply to player, only If they aren't past the apex of the jump while dampening is enabled
		if (!DampenMidairVelocity || (LastTimeOnFloor >= -JumpDuration))
		{
			PlayerRB.AddForce(targetXMovement * Vector2.right, ForceMode2D.Force);
		}
    }
	
	// Flip player about horizontal axis, returns new value if it is ever needed
	private bool Flip()
	{
		IsFacingRight = !IsFacingRight;
		
		// Scale values cannot be modified directly for some reason
		Vector3 NewScale = transform.localScale;
		NewScale.x *= -1.0f;
		transform.localScale = NewScale;
		
		return IsFacingRight;
	}
	
	// Returns the proper acceleration/deceleration values to be used by the movement calculations
	// The accleration simply needs to be calculated as such: ((1 / Time.fixedDeltaTime) * acceleration) / MaxRunningSpeed
	private float ProperAcceleration(float inputAcceleration)
	{
		return (50 * inputAcceleration) / MaxRunningSpeed; // Time.fixedDeltaTime is internally scaled by around 1/50
	}
}
