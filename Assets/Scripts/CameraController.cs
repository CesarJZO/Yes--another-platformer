using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float leftLimit;
    public float rightLimit;
    public Transform target;
    public float speed;
    public Vector3 offset;

    private void LateUpdate()
    {
        var desiredPosition = target.position + offset;
        desiredPosition.y = 0;
        var smooth = Vector3.Lerp(transform.position, desiredPosition, speed);
        smooth.x = Mathf.Clamp(smooth.x, leftLimit, rightLimit);
        transform.position = smooth;
    }
}
