using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CometSurface : MonoBehaviour
{
    private int heightScale = 20;
    public float detailScale = 0.001f;
    public GameObject treePrefab; // You can use a tree prefab for craters

    List<GameObject> craters = new List<GameObject>();
 
    void Start()
    {
        float seed;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

       seed = GameObject.Find("WorldGen").GetComponent<WorldGenerator>().seed;

        for (int v = 0; v < vertices.Length; v++)
        {
            float noiseValue = Mathf.PerlinNoise(
                (vertices[v].x + transform.position.x) * detailScale * seed,
                (vertices[v].z + transform.position.z) * detailScale * seed);

            // Add two additional layers of Perlin noise
            float noise1 = Mathf.PerlinNoise(
                (vertices[v].x + transform.position.x) * 0.01f,
                (vertices[v].z + transform.position.z) * 0.01f);

            float noise2 = Mathf.PerlinNoise(
                (vertices[v].x + transform.position.x) * 0.05f,
                (vertices[v].z + transform.position.z) * 0.05f);

            // Combine the three noise layers for added detail
            noiseValue += noise1 * 0.3f + noise2 * 0.2f;

            vertices[v].y = noiseValue * heightScale;

            if (noiseValue > 0.8f)
            {
                // Create a crater at this location
                CreateCrater(vertices[v]);
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        gameObject.AddComponent <MeshCollider>();
    }


    void CreateCrater(Vector3 position)
    {
        if (treePrefab != null)
        {
            GameObject newCrater = Instantiate(treePrefab, position, Quaternion.identity);
            craters.Add(newCrater);
        }
    }

    void OnDestroy()
    {
        foreach (GameObject crater in craters)
        {
            Destroy(crater);
        }
    }
     
}
