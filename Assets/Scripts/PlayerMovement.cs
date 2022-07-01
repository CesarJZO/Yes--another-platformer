using UnityEngine;
/// <summary>
/// PlayerMovement Class.
/// This class manages the movement of the player. 
/// </summary>
public class PlayerMovement : MonoBehaviour {
    /// <summary>
    ///  Amount of force added when the player jumps.
    /// </summary>
    [SerializeField] 
    private float jumpForce = 400f;                         
    /// <summary>
    /// How much to smooth out the movement
    /// </summary>
    [Range(0, .3f)] [SerializeField] 
    private float movementSmoothing = .05f;  
    /// <summary>
    /// Whether or not a player can steer while jumping;
    /// </summary>
    [SerializeField] 
    private bool airControl;                        
    /// <summary>
    /// A mask determining what is ground to the character.
    /// </summary>
    [SerializeField] 
    private LayerMask whatIsGround;                         
    /// <summary>
    /// A position marking where to check if the player is grounded.
    /// </summary>
    [SerializeField] 
    private Transform groundCheck;                           
    /// <summary>
    /// Radius of the overlap circle to determine if grounded.
    /// </summary>
    private const float GroundedRadius = .02f;
    /// <summary>
    /// Whether or not the player is grounded.
    /// </summary>
    public bool grounded;
    /// <summary>
    ///  Radius of the overlap circle to determine if the player can stand up
    /// </summary>
    private const float CeilingRadius = .2f;
    /// <summary>
    /// RigidBody reference.
    /// </summary>
    private Rigidbody2D _rigidbody2D;
    /// <summary>
    /// For determining which way the player is currently facing.
    /// </summary>
    private bool _facingRight = true;
    /// <summary>
    /// Specifies the velocity.
    /// </summary>
    private Vector2 _velocity = Vector3.zero;
    /// <summary>
    /// Awake Function.
    /// </summary>
    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// Fixed Update Function.
    /// </summary>
    private void FixedUpdate() {
        bool wasGrounded = grounded;
        grounded = false;
        // The player is grounded if a circle cast to the ground check position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        var colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject) {
                grounded = true;
            }
        }
    }
    /// <summary>
    /// Moves the player by a given direction.
    /// </summary>
    /// <param name="move"></param>
    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (!grounded && !airControl) return;
        // Move the character by finding the target velocity
        var velocity = _rigidbody2D.velocity;
        var targetVelocity = new Vector2(move * 10f, velocity.y);
        // And then smoothing it out and applying it to the character
        _rigidbody2D.velocity = Vector2.SmoothDamp(velocity, targetVelocity, ref _velocity, movementSmoothing);

        switch (move)
        {
            // If the input is moving the player right and the player is facing left...
            case > 0 when !_facingRight:
            // Otherwise if the input is moving the player left and the player is facing right...
            // ... flip the player.
            case < 0 when _facingRight:
                // ... flip the player.
                Flip();
                break;
        }
    }
    /// <summary>
    /// Flips the player scale.
    /// </summary>
    private void Flip() {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        var t = transform;
        var theScale = t.localScale;
        theScale.x *= -1;
        t.localScale = theScale;
    }
    /// <summary>
    /// Performs a player jump.
    /// </summary>
    public void Jump()
    {
        // If the player should jump...
        if (!grounded) return;
        // Add a vertical force to the player.
        grounded = false;
        _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
    }
}
