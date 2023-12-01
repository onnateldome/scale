using UnityEngine;

public class Damageable : MonoBehaviour
{
    // Method to take damage
    public void TakeDamage(int amount)
    {
        Debug.Log($"{gameObject.name} took {amount} damage!");
     
    }
}
