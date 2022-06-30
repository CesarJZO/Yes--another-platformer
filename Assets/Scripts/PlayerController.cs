using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private PlayerMovement _movement;
    private PlayerInput _playerInput;
    private InputAction _move;
    private InputAction _jump;


    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _move = _playerInput.actions[ActionName.Move.ToString()];
        _jump = _playerInput.actions[ActionName.Jump.ToString()];
    }

    private void FixedUpdate()
    {
        _movement.Move(_move.ReadValue<float>() * speed);
    }

    public void OnJump()
    {
        _movement.Jump();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Cherry"))
        {
            Destroy(col.gameObject);
        }
    }
}