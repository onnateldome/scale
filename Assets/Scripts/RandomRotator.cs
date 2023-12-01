using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    // Define the range of rotation speeds for each axis
    public Vector3 minRotationSpeed = new Vector3(10f, 10f, 10f);
    public Vector3 maxRotationSpeed = new Vector3(50f, 50f, 50f);

    // Update is called once per frame
    void Update()
    {
        // Calculate random rotation speeds for each axis
        float xSpeed = Random.Range(minRotationSpeed.x, maxRotationSpeed.x);
        float ySpeed = Random.Range(minRotationSpeed.y, maxRotationSpeed.y);
        float zSpeed = Random.Range(minRotationSpeed.z, maxRotationSpeed.z);

        // Apply rotation to the GameObject
        transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
    }
}
