using UnityEngine;

public class AsteroidSpawner1 : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnRadius = 50f;
    public int maxAsteroids = 20;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        if (GameObject.FindGameObjectsWithTag("Asteroid").Length >= maxAsteroids) return;

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Random.rotation);
        asteroid.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 5f;
    }
}