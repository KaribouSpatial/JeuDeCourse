using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.

	[Range(0, 2000)]
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.
	[Range(0, 2000)]
	[SerializeField] float lingeringForce = 20f;
	[Range(0, 2000)]
	[SerializeField] float jetpackForce = 10f;
	[Range(0, 2000)]
	[SerializeField] float horizontalJumpForce = 400f;

	[Range(0, 100)]
	[SerializeField] int maxJump = 0;
	[Range(0, 1)]
	[SerializeField] float jumpMaxPressTime = 0.5f;
	[SerializeField] bool maxJumpInfinite = false;

	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%

	[SerializeField] float airControl = 0.0f;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	bool onWall = false;

	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	//TODO:: WE DON'T NEED THIS BOOL FFS
	bool jetpackMode = false; //TODO:: KARIBOU ET MING UTILISEZ CA!!!!!
	// Vous pouvez le renommer en jetpackActive
	//I GIVE THE LOWEST AMOUNT OF FUCKS HUMANLY POSSIBLE

	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	float wallRadius = 0.05f;
	Animator anim;										// Reference to the player's animator component.
	uint jumpCount = 0;
	float elapsedJumping = 0.0f;
	Transform frontWallCheck;
	Transform backWallCheck;

	public bool Grounded { get { return grounded; } } 
	public bool OnWall   { get { return onWall; } }
	public bool JetpackMode { get { return jetpackMode; } }

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		frontWallCheck = transform.Find("FrontWallCheck");
		backWallCheck = transform.Find("BackWallCheck");
		anim = GetComponent<Animator>();
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if(grounded || airControl > 0.0f)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move * crouchSpeed : move);
			if(!grounded)
			{
				if(airControl >= 1.0f)
				{
					move *= airControl;
				}
				else
				{
					move = move * airControl + (rigidbody2D.velocity.x / maxSpeed) * (1.0f - airControl); 
				}
			}

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

		bool onFrontWall = Physics2D.OverlapCircle(frontWallCheck.position, wallRadius, whatIsGround);
		bool onBackWall = Physics2D.OverlapCircle(backWallCheck.position, wallRadius, whatIsGround);
		onWall =  onBackWall || onFrontWall;

		if (jump) 
		{
			//BOUCLE SPACE ENFONCE
			//GROUND JUMP START
			if (grounded || onWall) {
				jumpCount = 0;
			}

			// JUMP START
        	// If the player should jump...
			if (elapsedJumping == 0.0f && (maxJumpInfinite || jumpCount < maxJump)) 
			{
				elapsedJumping += Time.fixedDeltaTime;
				++jumpCount;
 				// Add a vertical force to the player.
				anim.SetBool ("Ground", false);
				if (onWall && !grounded) 
				{
					float direction = facingRight ? -1 : 1;
					direction *= onFrontWall ? 1 : -1;
					rigidbody2D.AddForce (new Vector2 (direction * horizontalJumpForce, jumpForce));
				}
				else 
				{
					rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
				}
			}
			// JUMP CONTINUE
			else
			{
				if(jumpCount > maxJump)
				{
					rigidbody2D.AddForce (jetpackForce);
				}
				else if((elapsedJumping + Time.fixedDeltaTime) < jumpMaxPressTime && !grounded)
				{
					elapsedJumping += Time.fixedDeltaTime;
					Vector2 proportionJump = Vector2.Lerp(new Vector2(0.0f, lingeringForce), Vector2.zero, elapsedJumping/jumpMaxPressTime);

					rigidbody2D.AddForce (proportionJump);
				}
			}
		} 
		else 
		{
			elapsedJumping = 0.0f;
		}
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
