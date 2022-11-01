using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(0f, 10f)]
    public float smoothFactor;
    public Vector3 minPos, maxPos;


    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 boundPos = new Vector3(
            Mathf.Clamp(targetPos.x, minPos.x, maxPos.x),
            Mathf.Clamp(targetPos.y, minPos.y, maxPos.y),
            Mathf.Clamp(targetPos.z, minPos.z, maxPos.z));

        Vector3 smoothPos = Vector3.Lerp(transform.position, boundPos, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}
