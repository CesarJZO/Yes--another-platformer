using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float dieForce;
    private float _horizontalInput;
    public bool isAlive = true;

    private PlayerMovement _movement;
    public GameManager gameManager;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private CircleCollider2D _circleCollider;

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
        _boxCollider = GetComponent<BoxCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
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
        var otherObject = col.gameObject;
        if (col.CompareTag("Cherry"))
        {
            Destroy(otherObject);
            gameManager.coins++;
        }

        if (col.CompareTag("Gem"))
        {
            Destroy(otherObject);
            gameManager.gems++;
        }

        if (col.CompareTag("Level End"))
        {
            gameManager.FinishLevel();
        }

        if (col.CompareTag("Poisoned"))
        {
            Destroy(otherObject);
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

    public void Respawn()
    {
        isAlive = true;
        _circleCollider.enabled = _boxCollider.enabled = true;
    }

    private void Die()
    {
        isAlive = false;
        _circleCollider.enabled = _boxCollider.enabled = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(Vector2.up * dieForce);
        _animator.SetTrigger(_dieAnimId);
    }
}
