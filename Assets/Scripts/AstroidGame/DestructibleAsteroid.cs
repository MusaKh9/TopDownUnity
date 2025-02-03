using UnityEngine;

public class DestructibleAsteroid : MonoBehaviour
{
    public int maxHealth = 3;
    public ParticleSystem explosionEffect;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}