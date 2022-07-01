using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool isAlive = true;
    private PlayerMovement _movement;

    private Rigidbody2D _rigidbody;
    private PlayerInput _playerInput;
    private InputAction _move;
    private InputAction _jump;


    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions[ActionName.Move.ToString()];
        _jump = _playerInput.actions[ActionName.Jump.ToString()];
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        _movement.Move(_move.ReadValue<float>() * speed);
    }

    public void OnJump()
    {
        if (!isAlive) return;
        _movement.Jump();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Cherry")) return;
        Destroy(col.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Spikes") || col.gameObject.CompareTag("Enemy"))
        {
            isAlive = false;
            _rigidbody.velocity = Vector2.zero;
        }
        else if (col.gameObject.CompareTag("Weak Point"))
        {
            var parent = col.transform.parent;
            parent.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(parent.gameObject);
        }
    }
}
