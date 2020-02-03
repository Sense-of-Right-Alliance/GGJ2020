using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemySpeedsterPrefab;
    [SerializeField] GameObject enemyPlainJanePrefab;
    [SerializeField] GameObject enemyBigBoiPrefab;
    [SerializeField] GameObject hugeAsteroidPrefab;
    [SerializeField] GameObject mediumAsteroidPrefab;
    [SerializeField] float spawnDelay = 0.5f; // seconds between each ship spawn in squadron
    [SerializeField] Transform enemyTopSpawnTransform;
    [SerializeField] Transform enemyBottomSpawnTransform;
    [SerializeField] Transform enemyLeftSpawnTransform;
    [SerializeField] Transform enemyRightSpawnTransform;
    [SerializeField] Transform asteroidTopSpawnTransform;
    [SerializeField] Transform asteroidBottomSpawnTransform;
    [SerializeField] float spawnWidth = 10f;

    private readonly Dictionary<SpawnPattern, int> _squadronSpawns = new Dictionary<SpawnPattern, int>
    {
        { SpawnPattern.Unknown, 0 },
        { SpawnPattern.Center, 1 },
        { SpawnPattern.FlyingV, 5 },
        { SpawnPattern.FlyingVInverted, 5 },
        { SpawnPattern.Column, 6 },
        { SpawnPattern.DoubleColumn, 6 },
        { SpawnPattern.Random, 6 },
    };

    private Dictionary<EnemyType, GameObject> _enemyPrefabs;

    private void Awake()
    {
        _enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Speedster, enemySpeedsterPrefab },
            { EnemyType.PlainJane, enemyPlainJanePrefab },
            { EnemyType.BigBoi, enemyBigBoiPrefab },
            { EnemyType.HugeAsteroid, hugeAsteroidPrefab },
            { EnemyType.MediumAsteroid, mediumAsteroidPrefab },
        };
    }

    private void Update()
    {

    }

    private IEnumerator SpawnCenter(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName)
    {
        Debug.Log("Spawning Center Formation");
        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Center]; i++)
        {
            var enemy = Instantiate(prefab, reference, rotation);
            enemy.tag = tagName;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnRandom(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName)
    {
        Debug.Log("Spawning Random 'Formation'");
        float radius = spawnWidth / 2.0f;

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Random]; i++)
        {
            var spawnPos = reference;
            spawnPos.x += UnityEngine.Random.Range(-radius, radius);

            var enemy = Instantiate(prefab, spawnPos, rotation);
            enemy.tag = tagName;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnFlyingV(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, bool inverted)
    {
        Debug.Log("Spawning Flying V Formation " + (inverted ? "(inverted)" : "(normal)"));
        float radius = spawnWidth / 2.0f;
        var numberOfSpawns = inverted ? _squadronSpawns[SpawnPattern.FlyingVInverted] : _squadronSpawns[SpawnPattern.FlyingV];

        float spacing = 0f;
        if (numberOfSpawns != 1)
        {
            spacing = radius / (numberOfSpawns - 1);
        }

        var spawnLocations = new List<List<Vector2>>(numberOfSpawns);
        for (var i = 0; i < numberOfSpawns; i++)
        {
            spawnLocations.Add(new List<Vector2>());

            var position = new Vector2(reference.x + spacing * i, reference.y);
            spawnLocations[i].Add(position);

            if (i > 0)
            {
                var mirroredPosition = new Vector2(reference.x - spacing * i, reference.y);
                spawnLocations[i].Add(mirroredPosition);
            }
        }

        if (inverted)
        {
            spawnLocations.Reverse();
        }

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            for (int j = 0; j < spawnLocations[i].Count; j++)
            {
                var spawnPos = spawnLocations[i][j];
                var enemy = Instantiate(prefab, spawnPos, rotation);
                enemy.tag = tagName;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void Spawn(Squadron squadron)
    {
        Vector2 referenceVector;
        Quaternion rotation;
        string tagName = "Enemy";
        switch (squadron.SpawnZone)
        {
            case SpawnZone.Bottom:
                referenceVector = enemyBottomSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 180f);
                tagName = "AmbushEnemy";
                break;
            case SpawnZone.Left:
                referenceVector = enemyLeftSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 270f);
                break;
            case SpawnZone.Right:
                referenceVector = enemyBottomSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 90f);
                break;
            case SpawnZone.TopAsteroid:
                referenceVector = asteroidTopSpawnTransform.position;
                rotation = Quaternion.identity;
                break;
            case SpawnZone.BottomAsteroid:
                referenceVector = asteroidBottomSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 180f);
                break;
            case SpawnZone.Top:
            case SpawnZone.Unknown:
            default:
                referenceVector = enemyTopSpawnTransform.position;
                rotation = Quaternion.identity;
                break;
        }

        if (squadron.EnemyType == EnemyType.HugeAsteroid || squadron.EnemyType == EnemyType.MediumAsteroid)
        {
            tagName = "Asteroid";
        }

        switch (squadron.SpawnPattern)
        {
            case SpawnPattern.Center:
                StartCoroutine(SpawnCenter(referenceVector, squadron.EnemyType, rotation, tagName));
                break;
            case SpawnPattern.Random:
                StartCoroutine(SpawnRandom(referenceVector, squadron.EnemyType, rotation, tagName));
                break;
            case SpawnPattern.FlyingV:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tagName, false));
                break;
            case SpawnPattern.FlyingVInverted:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tagName, true));
                break;
        }
    }
}
