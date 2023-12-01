using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

    public int damage = 10;

	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 100f;
	
	}

    private void OnCollisionEnter(Collision collision)
    {
        // Get the GameObject that the bullet collided with
        GameObject otherObject = collision.gameObject;

        // Check if the collided object has a component that can take damage
        Damageable damageable = otherObject.GetComponent<Damageable>();

        // If the collided object can take damage, apply the damage
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        // Destroy the bullet after colliding with any object
        Destroy(gameObject);
    }

}
