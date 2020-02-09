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
    [SerializeField] GameObject enemyCrabLeftPrefab;
    [SerializeField] GameObject enemyCrabRightPrefab;
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

    private List<GameObject> _enemySpawns = new List<GameObject>();

    public int NumEnemies { get { return _enemySpawns.Count; } }

    private void Awake()
    {
        _enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Speedster, enemySpeedsterPrefab },
            { EnemyType.PlainJane, enemyPlainJanePrefab },
            { EnemyType.BigBoi, enemyBigBoiPrefab },
            { EnemyType.CrabLeft, enemyCrabLeftPrefab },
            { EnemyType.CrabRight, enemyCrabRightPrefab },
            { EnemyType.HugeAsteroid, hugeAsteroidPrefab },
            { EnemyType.MediumAsteroid, mediumAsteroidPrefab },
        };
    }

    private void Update()
    {

    }

    private void OnEnemyDestroyedOrRemoved(GameObject enemy)
    {
        _enemySpawns.Remove(enemy);

        Enemy eComp = enemy.GetComponent<Enemy>();
        Asteroid aComp = enemy.GetComponent<Asteroid>();

        if (eComp) eComp.EnemyDestroyedOrRemovedEvent.RemoveListener(OnEnemyDestroyedOrRemoved);
        else if (aComp) aComp.EnemyDestroyedOrRemovedEvent.RemoveListener(OnEnemyDestroyedOrRemoved);

        Debug.Log("Removed! Num Spawns = " + _enemySpawns.Count);
    }

    private void SpawnEnemy(GameObject prefab, Vector2 spawnPos, Quaternion rotation, string tagName)
    {
        var enemy = Instantiate(prefab, spawnPos, rotation);
        enemy.tag = tagName;
        _enemySpawns.Add(enemy);

        Enemy eComp = enemy.GetComponent<Enemy>();
        Asteroid aComp = enemy.GetComponent<Asteroid>();
        if (eComp) eComp.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);
        else if (aComp) aComp.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);

        Debug.Log("Spawned! Num Spawns = " + _enemySpawns.Count);
    }

    private IEnumerator SpawnColumn(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName)
    {
        Debug.Log("Spawning Column Formation");

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Column]; i++)
        {
            var spawnPos = reference;

            SpawnEnemy(prefab, spawnPos, rotation, tagName);

            yield return new WaitForSeconds(spawnDelay * 2.5f);
        }
    }

    private IEnumerator SpawnCenter(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName)
    {
        Debug.Log("Spawning Center Formation");
        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Center]; i++)
        {
            SpawnEnemy(prefab, reference, rotation, tagName);

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

            SpawnEnemy(prefab, spawnPos, rotation, tagName);

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
            spacing = spawnWidth / (numberOfSpawns - 1);
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
                SpawnEnemy(prefab, spawnPos, rotation, tagName);
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
                rotation = Quaternion.Euler(0, 0, 90f);
                break;
            case SpawnZone.Right:
                referenceVector = enemyRightSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 270f);
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
            case SpawnPattern.Column:
                StartCoroutine(SpawnColumn(referenceVector, squadron.EnemyType, rotation, tagName));
                break;
        }
    }
}
