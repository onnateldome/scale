using UnityEngine;

public class MouseClickRaycaster : MonoBehaviour
{
    public MeshFilter meshFilter; // Reference to the MeshFilter component with the mesh you want to modify.
    public float scaleFactor = 1.001f; // The scaling factor.
    private Mesh originalMesh;

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about the hit
            RaycastHit hit;

            // Perform the raycast and check if it hits something
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a MeshRenderer (to make sure it's a visible object)
                MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    // The ray hit a mesh, you can now interact with it
                    Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    meshFilter = hit.collider.gameObject.GetComponent<MeshFilter>();
                    originalMesh = Instantiate(meshFilter.sharedMesh); // Create a copy of the original mesh.
                    meshFilter.mesh = originalMesh;

                    // Find the closest vertices to the hit point.
                    Vector3[] vertices = originalMesh.vertices;

                    for (int i = 0; i < vertices.Length; i++)
                    {
                        Vector3 worldVertexPosition = meshFilter.transform.TransformPoint(vertices[i]);

                        // Calculate the distance from the vertex to the hit point.
                        float distance = Vector3.Distance(worldVertexPosition, hit.point);

                        // Determine which side of the object the vertex belongs to.
                        if (worldVertexPosition.x < hit.point.x)
                        {
                            // Scale the vertices on the side of the hit.
                            vertices[i] *= scaleFactor;
                        }
                    }

                    // Assign the modified vertices back to the mesh.
                    originalMesh.vertices = vertices;

                    // Recalculate normals and bounds if necessary.
                    originalMesh.RecalculateNormals();
                    originalMesh.RecalculateBounds();

                    // You can also do further actions with the hit object, such as highlighting it or selecting it.
                }
            }
        }
    }
}
