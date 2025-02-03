using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 30f;
    public float lifetime = 3f;
    public int damage = 1;
    public float homingStrength = 5f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifetime);
        FindClosestAsteroid();
    }

    void Update()
    {
        if (target != null)
        {
            // Homing towards target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                 Quaternion.LookRotation(direction),
                                                 homingStrength * Time.deltaTime);
        }

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void FindClosestAsteroid()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject asteroid in asteroids)
        {
            float distance = Vector3.Distance(transform.position, asteroid.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = asteroid.transform;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            DestructibleAsteroid asteroid = other.GetComponent<DestructibleAsteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}