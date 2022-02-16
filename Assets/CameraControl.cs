using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraControl : MonoBehaviour
{
    public float maxSize, minSize;
    public float speed = 1;

    private float targetSize;
    Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
            targetSize += 0.1f*speed;
        else if (Input.mouseScrollDelta.y > 0)
            targetSize -= 0.1f * speed;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, 0.1f);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
    }

    private void ZoomIn()
    {

    }

    private void ZoomOut()
    {

    }
}
