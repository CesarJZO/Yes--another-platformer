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
    private float m_JumpForce = 400f;                         
    /// <summary>
    /// How much to smooth out the movement
    /// </summary>
    [Range(0, .3f)] [SerializeField] 
    private float m_MovementSmoothing = .05f;  
    /// <summary>
    /// Whether or not a player can steer while jumping;
    /// </summary>
    [SerializeField] 
    private bool m_AirControl = false;                        
    /// <summary>
    /// A mask determining what is ground to the character.
    /// </summary>
    [SerializeField] 
    private LayerMask m_WhatIsGround;                         
    /// <summary>
    /// A position marking where to check if the player is grounded.
    /// </summary>
    [SerializeField] 
    private Transform m_GroundCheck;                           
    /// <summary>
    /// Radius of the overlap circle to determine if grounded.
    /// </summary>
    const float k_GroundedRadius = .2f;
    /// <summary>
    /// Whether or not the player is grounded.
    /// </summary>
    public bool m_Grounded;
    /// <summary>
    ///  Radius of the overlap circle to determine if the player can stand up
    /// </summary>
    const float k_CeilingRadius = .2f;
    /// <summary>
    /// RigidBody reference.
    /// </summary>
    private Rigidbody2D m_Rigidbody2D;
    /// <summary>
    /// For determining which way the player is currently facing.
    /// </summary>
    private bool m_FacingRight = true;
    /// <summary>
    /// Specifies the velocity.
    /// </summary>
    private Vector3 m_Velocity = Vector3.zero;
    /// <summary>
    /// Awake Function.
    /// </summary>
    private void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// Fixed Update Function.
    /// </summary>
    private void FixedUpdate() {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;
            }
        }
    }
    /// <summary>
    /// Moves the player by a given direction.
    /// </summary>
    /// <param name="move"></param>
    public void Move(float move) {
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }
    /// <summary>
    /// Flips the player scale.
    /// </summary>
    private void Flip() {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    /// <summary>
    /// Performs a player jump.
    /// </summary>
    public void Jump() {
        // If the player should jump...
        if (m_Grounded)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }
}