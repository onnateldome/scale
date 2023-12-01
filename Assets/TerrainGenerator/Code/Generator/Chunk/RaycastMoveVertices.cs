using UnityEngine;

public class RaycastMoveVertices : MonoBehaviour
{
    public float raycastDistance = 10f;
    public float moveDistance = 0.1f;
    public int numClosestVerticesToMove = 10;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();

                if (meshCollider != null && meshFilter != null)
                {
                    // Get the mesh data
                    Mesh originalMesh = meshFilter.mesh;
                    Mesh meshCopy = Instantiate(originalMesh); // Create a copy of the original mesh
                    Vector3[] vertices = meshCopy.vertices;

                    // Calculate the hit point in local coordinates of the mesh
                    Vector3 hitPointLocal = meshCollider.transform.InverseTransformPoint(hit.point);

                    // Find the closest vertices to the hit point
                    int[] closestVertices = FindClosestVertices(vertices, hitPointLocal, numClosestVerticesToMove);

                    // Move the closest vertices along the ray
                    for (int i = 0; i < closestVertices.Length; i++)
                    {
                        int vertexIndex = closestVertices[i];
                        vertices[vertexIndex] += ray.direction * moveDistance;
                    }

                    // Update the copy of the mesh with the modified vertices
                    meshCopy.vertices = vertices;
                    meshCopy.RecalculateBounds();
                    meshCopy.RecalculateNormals();

                    // Set the modified mesh as the current mesh for rendering
                    meshFilter.mesh = meshCopy;

                    // Update the mesh collider with the modified mesh
                    meshCollider.sharedMesh = meshCopy;
                }
            }
        }
    }

    // Function to find the closest vertices to a given point
    private int[] FindClosestVertices(Vector3[] vertices, Vector3 point, int numVertices)
    {
        int[] closestVertices = new int[numVertices];
        float[] distances = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            distances[i] = Vector3.Distance(vertices[i], point);
        }

        for (int i = 0; i < numVertices; i++)
        {
            int closestVertexIndex = -1;
            float closestDistance = float.MaxValue;

            for (int j = 0; j < distances.Length; j++)
            {
                if (distances[j] < closestDistance)
                {
                    closestDistance = distances[j];
                    closestVertexIndex = j;
                }
            }

            if (closestVertexIndex != -1)
            {
                closestVertices[i] = closestVertexIndex;
                distances[closestVertexIndex] = float.MaxValue; // Mark this vertex as used
            }
        }

        return closestVertices;
    }
}
