using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(0, 10)]
    public float smoothingSpeed;
    public Vector3 offset;

    public float magnitude;

    public float swayIntensity;
    public float maxSway;
    Transform target;
    Vector3 additionalOffset;
    Vector3 swayOffset;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 finalMovement = target.position + offset + additionalOffset + swayOffset;
        transform.position = Vector3.Lerp(transform.position, finalMovement + transform.forward, Time.deltaTime * smoothingSpeed);

        transform.position += new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
    }

    public void SetTarget(Transform target) => this.target = target;

    public void SetOffset(Vector3 offset) => additionalOffset = offset;

    public void SetShakeMagnitude(float magnitude) => this.magnitude = magnitude;
}
