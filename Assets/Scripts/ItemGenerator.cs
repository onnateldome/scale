using System;
using System.Collections;
using System.Collections.Generic;
using TerrainGenerator;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ItemGenerator : MonoBehaviour
{

    public List<GameObject> Items = new List<GameObject>();
    public List<GameObject> ActiveItems = new List<GameObject>();
    public List<int> itemPercentages = new List<int>();
    public List<bool> isFloating = new List<bool>();

    public Transform observer;

    public float spawnInterval = 5f;
    public float spawnDistance = 100f;
    public int maxItems = 100;
    private float timer = 0f;
    public float heightAboveTerrain = 10f;
    public TerrainChunkGenerator Generator;

    public void Start()
    {
        Generator = GameObject.Find("TerrainGenerator").GetComponent<TerrainChunkGenerator>();
    }

    // Update is called once per frame
    public void Update()
    {
        ActiveItems.RemoveAll(item => item == null);
        ActiveItems.RemoveAll(item => item.gameObject.active == false);

        if (ActiveItems.Count < maxItems)
        {
            Vector3 playerDirection = observer.forward;
            Vector3 spawnPosition = observer.position + playerDirection * spawnDistance;
            if (Generator.IsTerrainAvailable(spawnPosition))
            {
                spawnPosition.x += UnityEngine.Random.Range(-70f, 70f);
                spawnPosition.z += UnityEngine.Random.Range(0f, 30f);
                spawnPosition.y = Generator.GetTerrainHeight(spawnPosition);
            }
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                // Spawn enemy at the calculated position
                SpawnItem(spawnPosition);

                // Reset the timer
                timer = 0f;
            }
        }

        // Iterate backward through the collection to avoid modification issues
        for (int i = ActiveItems.Count - 1; i >= 0; i--)
        {
            GameObject obj = ActiveItems[i];
            if (obj != null)
            {
                if (IsObjectBehind(obj.transform))
                {
                    ActiveItems.RemoveAt(i);
                    Destroy(obj);
                }
            }
        }
    }



    bool IsObjectBehind(Transform target)
    {
        // Calculate the vector from the observer to the target
        Vector3 toTarget = target.position - observer.position;

        // Use the dot product to check if the target is behind the observer
        float dotProduct = Vector3.Dot(toTarget.normalized, observer.forward);

        // If dot product is negative, the target is behind the observer
        return dotProduct < 0f;
    }
    void SpawnItem(Vector3 position)
    {
        int totalWeight = 0;

        // Calculate the total weight of all items
        foreach (var percentage in itemPercentages)
        {
            totalWeight += percentage;
        }

        // Generate a random value between 0 and the total weight
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        int cumulativeWeight = 0;

        // Iterate through the items and select the one corresponding to the random value
        for (int i = 0; i < Items.Count; i++)
        {
            cumulativeWeight += itemPercentages[i];

            if (randomValue < cumulativeWeight)
            {
                if (isFloating[i]) { position.y += heightAboveTerrain + UnityEngine.Random.Range(0f, 20f); }
                GameObject newItem = Instantiate(Items[i], position, Quaternion.identity);
                ActiveItems.Add(newItem);
                break; // Exit the loop after spawning the item
            }
        }
    }


}
