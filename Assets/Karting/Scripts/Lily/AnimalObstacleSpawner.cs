using UnityEngine;

public class AnimalObstacleSpawner : MonoBehaviour
{
	public GameObject[] animalPrefabs;
	public Transform[] spawnPoints;
	public float spawnInterval = 5f;
	public float animalSpeed = 5f;
	public float animalLifetime = 10f;

	private bool isActive = false;

	public AudioSource audioSource;
	public AudioClip spawnSound;

	public void ActivateAnimals()
	{
		isActive = true;
		InvokeRepeating(nameof(SpawnAnimal), 1f, spawnInterval);
	}

	public void DeactivateAnimals()
	{
		isActive = false;
		CancelInvoke(nameof(SpawnAnimal));
	}

	void SpawnAnimal()
	{
		if (!isActive || spawnPoints.Length == 0 || animalPrefabs.Length == 0) return;

		Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
		GameObject prefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];
		GameObject animal = Instantiate(prefab, spawn.position, spawn.rotation);
		animal.GetComponent<Rigidbody>().velocity = spawn.right * animalSpeed;

		if (audioSource && spawnSound)
			audioSource.PlayOneShot(spawnSound);

		Destroy(animal, animalLifetime);
	}

}
