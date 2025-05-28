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
	public AudioClip spawnDog;
	public AudioClip spawnCat;


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

		if (audioSource)
		{
			// Check prefab name and play the corresponding sound
			if (prefab.name.ToLower().Contains("dog") && spawnDog != null)
			{
				audioSource.PlayOneShot(spawnDog);
			}
			else if (prefab.name.ToLower().Contains("cat") && spawnCat != null)
			{
				audioSource.PlayOneShot(spawnCat);
			}
		}

		Destroy(animal, animalLifetime);
	}


}
