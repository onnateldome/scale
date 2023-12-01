using System.Collections;
using TerrainGenerator;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 1f;
    public float distance = 5.0f;
    public float heightAboveTerrain = 15.0f;

    private bool isMoving = false;
    TerrainChunkGenerator Generator;

    private void Start()
    {
        Generator = GameObject.Find("TerrainGenerator").GetComponent<TerrainChunkGenerator>();
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
                Vector3 targetPosition = transform.position + transform.forward * distance + randomDirection * distance;
                if (Generator.IsTerrainAvailable(targetPosition))
                {
                    targetPosition.y = Generator.GetTerrainHeight(targetPosition) + heightAboveTerrain;
                }
              

                // Rotate the camera to look at the destination
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

                // Move the camera to the destination
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {                    
                   // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                   // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                   
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                   
                    if (Generator.IsTerrainAvailable(transform.position))
                        if (transform.position.y  < Generator.GetTerrainHeight(transform.position) +heightAboveTerrain )
                    {
                        transform.position = new Vector3(transform.position.x, Generator.GetTerrainHeight(transform.position) + heightAboveTerrain, transform.position.z);
                    }

                    yield return null;
                }

                isMoving = false;
            }

            yield return null;
        }
    }
}
