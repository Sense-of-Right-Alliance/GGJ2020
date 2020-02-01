using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Exterior.Enemies
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject enemyFighterPrefab;
        [SerializeField] float spawnRate = 3f; // ship per second
        [SerializeField] Transform enemyTopSpawnTransform;
        [SerializeField] Transform enemyBottomSpawnTransform;
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
                Vector2 spawnPos = enemyTopSpawnTransform.position;
                spawnPos.x += UnityEngine.Random.Range(-radius, radius);

                GameObject.Instantiate(enemyFighterPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
