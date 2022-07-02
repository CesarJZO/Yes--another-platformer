using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float distance;
    
    
    private float _leftPos;
    private float _rightPos;
    private bool _isMovingRight = true;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        var position = transform.position;
        _leftPos = position.x - distance;
        _rightPos = position.x + distance;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = true;
    }

    private void Update()
    {
        if (transform.position.x >= _rightPos)
            _spriteRenderer.flipX = _isMovingRight = false;

        if (transform.position.x <= _leftPos)
            _spriteRenderer.flipX = _isMovingRight = true;
        
        var direction = _isMovingRight ? Vector3.right : Vector3.left;
        transform.Translate(direction * (speed * Time.deltaTime));
    }
}
