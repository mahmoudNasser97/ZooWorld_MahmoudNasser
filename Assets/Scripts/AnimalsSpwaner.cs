using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsSpwaner : MonoBehaviour
{
    public GameObject snakePrefab;
    public GameObject frogPrefab;
    public Transform[] spawnPoints; 
    public float spawnInterval = 3f; 

    private void Start()
    {
        StartCoroutine(SpawnAnimals());
    }

    private IEnumerator SpawnAnimals()
    {
        while (true)
        {
            SpawnAnimal(snakePrefab);
            yield return new WaitForSeconds(spawnInterval);

            SpawnAnimal(frogPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnAnimal(GameObject animalPrefab)
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(animalPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
