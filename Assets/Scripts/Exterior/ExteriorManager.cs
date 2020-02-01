using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExteriorManager : MonoBehaviour
{
    [SerializeField] GameObject enemyFighterPrefab;
    [SerializeField] float spawnRate = 3f; // ship per second
    [SerializeField] Transform enemySpawnTransform;
    [SerializeField] float spawnWidth = 10f;

    float spawnTimer = 0f;

    private void Start()
    {
        spawnTimer = spawnRate;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnRate;

            float radius = spawnWidth / 2.0f;
            Vector2 spawnPos = enemySpawnTransform.position;
            spawnPos.x += Random.Range(-radius, radius);

            GameObject.Instantiate(enemyFighterPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void HandleShipDestroyed(Ship ship)
    {
        // TODO: Handle game over
        Debug.Log("ExteriorManager -> Game Over!");
    }
}
