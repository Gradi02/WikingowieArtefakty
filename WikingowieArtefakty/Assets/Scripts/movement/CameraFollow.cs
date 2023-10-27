using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Transform camTransform;
    public Vector3 Offset;
    public Vector3 Rotation;
    public float SmoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        // update position
        Vector3 targetPosition = Target.position + Offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        camTransform.rotation = Quaternion.Euler(Rotation);
    }
}
