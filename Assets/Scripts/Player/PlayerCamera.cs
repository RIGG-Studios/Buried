using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private float smoothingSpeed;

    [SerializeField]
    private Vector3 offset;
    
    Transform target;
    Vector3 additionalOffset;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset + 
            additionalOffset, Time.deltaTime * smoothingSpeed);
    }

    public void SetTarget(Transform target) => this.target = target;

    public void SetOffset(Vector3 offset) => additionalOffset = offset;
}
