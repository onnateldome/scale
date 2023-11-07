using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5.0f; // Adjust the speed of enemy movement as needed
    public float rotationSpeed = 2.0f; // Adjust the rotation speed as needed
    public Transform target; // The target to follow (another enemy)
    public bool followTarget = false; // Enable or disable following the target
    public float spiralRadius = 5.0f; // Radius of the spiral movement
    public float spiralSpeed = 2.0f; // Speed of the spiral movement

    void Start()
    {
        if (followTarget && target == null)
        {
            // If following a target is enabled, but the target is not assigned, disable following.
            followTarget = false;
        }
    }

    void Update()
    {
        if (followTarget)
        {
            if (target != null)
            {
                // Calculate the direction to the target
                Vector3 direction = (target.position - transform.position).normalized;

                // Rotate towards the target
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // If the target is lost or destroyed, stop following.
                followTarget = false;
            }
        }
        else
        {
            // Perform a spiral movement
            float angle = Time.time * spiralSpeed;
            Vector3 spiralPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spiralRadius;
            transform.position = spiralPosition;
        }

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
