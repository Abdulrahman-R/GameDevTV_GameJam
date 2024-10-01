using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public bool rotateClockwise = true; // Direction of rotation

    void Update()
    {
        float direction = rotateClockwise ? 1f : -1f;
        transform.Rotate(Vector3.forward, direction * rotationSpeed * Time.deltaTime);
    }

    // Method to set rotation direction to clockwise
    public void SetClockwise(bool isClockwise)
    {
        rotateClockwise = isClockwise;
    }
}
