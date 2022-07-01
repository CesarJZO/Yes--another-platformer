using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool isAlive = true;
    private PlayerMovement _movement;

    private Rigidbody2D _rigidbody;
    public GameManager gameManager;
    
    private InputAction _move;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _move = GetComponent<PlayerInput>().actions[ActionName.Move.ToString()];
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
        _rigidbody.velocity = Vector2.zero;
    }
}
