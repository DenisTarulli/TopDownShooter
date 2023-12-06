using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    [Header("References")]
    [SerializeField] private SpawnableObject[] objects;
    [SerializeField] private GameObject spawnAnimation;

    [Header("Variables")]
    public float minSpawnRate = 0.75f;
    public float maxSpawnRate = 2f;

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        foreach (var obj in objects)
        {
            if (spawnChance < obj.spawnChance)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-17f, 17f), Random.Range(-17f, 17f), 0f);
                StartCoroutine(SpawnAnimation(spawnPosition, obj.prefab));
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private IEnumerator SpawnAnimation(Vector3 spawnPosition, GameObject obj)
    {
        GameObject spawnEffect = Instantiate(spawnAnimation, spawnPosition, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);

        Instantiate(obj, spawnPosition, Quaternion.identity);
        Destroy(spawnEffect);
    }
}
