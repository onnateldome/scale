using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public float distance = 10.0f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        StartCoroutine(CameraMovement());
    }


    private IEnumerator CameraMovement()
    {
        while (true)
        {
            if (!isMoving)
            {
                isMoving = true;

                // Generate a random point in front of the camera
                Vector3 randomDirection = Random.insideUnitSphere;
                targetPosition = transform.position + transform.forward * distance + randomDirection * distance;
                targetPosition.y = 20;              

                // Rotate the camera to look at the destination
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
              

                // Move the camera to the destination
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                    yield return null;
                }

                isMoving = false;

            }

            yield return null;
        }
    }
}
