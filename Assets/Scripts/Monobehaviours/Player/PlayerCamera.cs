using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private float smoothingSpeed;

    [SerializeField]
    private Vector3 offset;

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
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 5f);
    }

    public void SetTarget(Transform target) => this.target = target;

    public void SetOffset(Vector3 offset) => additionalOffset = offset;
}
