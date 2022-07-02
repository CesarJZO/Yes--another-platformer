using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float _horizontalInput;
    public bool isAlive = true;

    private PlayerMovement _movement;
    private Rigidbody2D _rigidbody;
    public GameManager gameManager;
    
    private InputAction _move;

    #region Animation

    private Animator _animator;
    private int _moveAnimId;
    private int _jumpAnimId;
    private int _groundAnimId;
    private int _isAliveAnimId;
    private int _dieAnimId;
    private int _vertAnimId;

    #endregion

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _move = GetComponent<PlayerInput>().actions[ActionName.Move.ToString()];
        
        _animator = GetComponent<Animator>();
        _moveAnimId = Animator.StringToHash("Move");
        _jumpAnimId = Animator.StringToHash("Jump");
        _groundAnimId = Animator.StringToHash("Grounded");
        _isAliveAnimId = Animator.StringToHash("Is Alive");
        _dieAnimId = Animator.StringToHash("Die");
        _vertAnimId = Animator.StringToHash("Vertical Velocity");
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        _horizontalInput = _move.ReadValue<float>(); 
        _movement.Move(_horizontalInput * speed);
    }

    private void LateUpdate()
    {
        _animator.SetBool(_moveAnimId, Mathf.Abs(_horizontalInput) > 0.01);
        _animator.SetFloat(_vertAnimId, _rigidbody.velocity.y);
        _animator.SetBool(_groundAnimId, _movement.grounded);
        _animator.SetBool(_isAliveAnimId, isAlive);
    }

    public void OnJump()
    {
        if (!isAlive) return;
        _movement.Jump();
        _animator.SetTrigger(_jumpAnimId);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Cherry"))
        {
            Destroy(col.gameObject);
            gameManager.coins++;
        }
        if (col.CompareTag("Level End"))
        {
            gameManager.FinishLevel();
            Die();
        }
        if (col.CompareTag("Poisoned"))
        {
            Destroy(col.gameObject);
            Die();
        }
        if (col.CompareTag("Checkpoint"))
            gameManager.spawnPoint.position = col.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Weak Point"))
        {
            var parent = col.transform.parent;
            parent.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(parent.gameObject);
        }
        if (col.gameObject.CompareTag("Spikes") || col.gameObject.CompareTag("Enemy"))
            Die();
    }

    private void Die()
    {
        isAlive = false;
        _animator.SetTrigger(_dieAnimId);
        _rigidbody.velocity = Vector2.zero;
    }
}
