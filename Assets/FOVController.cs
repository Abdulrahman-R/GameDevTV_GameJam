using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FOVController : MonoBehaviour
{
    public GameObject fovPivot;
    public Transform target;
    public float rotationSpeed = 5f;

    void Start()
    {
        target = null;
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;

            // Determine the angle to rotate based on the direction
            float angle = 0f;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    // Target is to the right
                    angle = 90f;
                }
                else
                {
                    // Target is to the left
                    angle = -90f;
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    // Target is above
                    angle = 180f;
                }
                else
                {
                    // Target is below
                    angle = 0f;
                }
            }

            // Smoothly rotate the fovPivot to the target angle
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            fovPivot.transform.rotation = Quaternion.Slerp(fovPivot.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }


    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
