using UnityEngine;

public class OncomingTrafficSpawner : MonoBehaviour
{
	public GameObject[] carPrefabs;
	public Transform[] spawnPoints;
	public float spawnInterval = 4f;
	public float carSpeed = 25f;
	public float carLifetime = 10f;

	private bool isActive = false;

	public AudioSource audioSource;
	public AudioClip trafficSpawnClip;

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

		float variedSpeed = carSpeed * Random.Range(0.9f, 1.1f);
		rb.velocity = -spawn.forward * variedSpeed;

		if (audioSource && trafficSpawnClip)
			audioSource.PlayOneShot(trafficSpawnClip);

		Destroy(car, carLifetime);
	}


	public void DeactivateTraffic()
	{
		isActive = false;
		CancelInvoke(nameof(SpawnCar));
	}
}
