using UnityEngine;

public class DestroyAsteroid : MonoBehaviour
{
    public int health = 3;
    public ParticleSystem explosionEffect;

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}