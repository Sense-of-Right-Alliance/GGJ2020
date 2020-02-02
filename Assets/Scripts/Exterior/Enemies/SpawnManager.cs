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
    [SerializeField] float spawnDelay = 0.5f; // seconds between each ship spawn in squadron
    [SerializeField] Transform enemyTopSpawnTransform;
    [SerializeField] Transform enemyBottomSpawnTransform;
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

    private void Start()
    {
        _enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Speedster, enemySpeedsterPrefab },
            { EnemyType.PlainJane, enemyPlainJanePrefab },
            { EnemyType.BigBoi, enemyBigBoiPrefab },
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
        string tag;
        switch (squadron.SpawnZone)
        {
            case SpawnZone.Bottom:
                referenceVector = enemyBottomSpawnTransform.position;
                rotation = Quaternion.Euler(0, 0, 180f);
                tag = "AmbushEnemy";
                break;
            case SpawnZone.Top:
            case SpawnZone.Unknown:
            default:
                referenceVector = enemyTopSpawnTransform.position;
                rotation = Quaternion.identity;
                tag = "Enemy";
                break;
        }

        switch (squadron.SpawnPattern)
        {
            case SpawnPattern.Center:
                StartCoroutine(SpawnCenter(referenceVector, squadron.EnemyType, rotation, tag));
                break;
            case SpawnPattern.Random:
                StartCoroutine(SpawnRandom(referenceVector, squadron.EnemyType, rotation, tag));
                break;
            case SpawnPattern.FlyingV:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tag, false));
                break;
            case SpawnPattern.FlyingVInverted:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tag, true));
                break;
        }
    }
}
