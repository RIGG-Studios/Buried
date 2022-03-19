using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    [Range(0, 10)]
    public float smoothingSpeed;
    public Vector3 offset;

    float magnitude;
    Transform target;
    Vector3 additionalOffset;
    Vector3 shakeOffset;
    

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 finalMovement = target.position + offset + additionalOffset + shakeOffset;
        transform.position = Vector3.Lerp(transform.position, finalMovement + transform.forward, Time.deltaTime * smoothingSpeed);

        transform.position += new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude));
    }
    public void SetTarget(Transform target) => this.target = target;

    public void SetOffset(Vector3 offset) => additionalOffset = offset;

    public void SetShakeMagnitude(float magnitude) => this.magnitude = magnitude;

    public void ShakeCamera(float dur, float mag) => StartCoroutine(Shake(dur, mag));
    private IEnumerator Shake(float dur, float mag)
    {
        float t = 0.0f;

        while(t < dur)
        {
            float x = Random.Range(-1f, 1f) * mag;
            float y = Random.Range(-1f, 1f) * mag;

            shakeOffset = new Vector3(x, y);
            t += Time.deltaTime;

            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
