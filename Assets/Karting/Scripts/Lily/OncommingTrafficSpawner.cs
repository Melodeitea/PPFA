using UnityEngine;

public class OncomingTrafficSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 4f;
    public float carSpeed = 25f;
    public float carLifetime = 10f;

    private bool isActive = false;

    public void ActivateTraffic()
    {
        isActive = true;
        InvokeRepeating(nameof(SpawnCar), 1f, spawnInterval);
    }

    void SpawnCar()
    {
        if (!isActive || carPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawn.position, spawn.rotation);
        Rigidbody rb = car.GetComponent<Rigidbody>();
        rb.velocity = -spawn.forward * carSpeed;

        Destroy(car, carLifetime);
    }

    public void DeactivateTraffic()
    {
        isActive = false;
        CancelInvoke(nameof(SpawnCar));
    }
}
