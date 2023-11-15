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
    private float NormalSmooth;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        NormalSmooth = SmoothTime;
    }

    private void FixedUpdate()
    {
        if (Target == null) return;

        // update position
        Vector3 targetPosition = Target.position + Offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        camTransform.rotation = Quaternion.Euler(Rotation);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos + Offset;
    }

    public void ResetSmooth()
    {
        SmoothTime = NormalSmooth;
    }
}

