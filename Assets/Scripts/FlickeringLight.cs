using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light spotlight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 1f;

    void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light>();
        }

        // Start the flickering coroutine
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // Randomly change the intensity within the specified range
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            spotlight.intensity = randomIntensity;

            // Wait for a short duration before flickering again
            yield return new WaitForSeconds(1 / flickerSpeed);
        }
    }
}
