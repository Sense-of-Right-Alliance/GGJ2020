using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemySpeedsterPrefab;
    [SerializeField] float spawnDelay = 0.3f; // seconds between each ship spawn in squadron
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
            { EnemyType.Speedster, enemySpeedsterPrefab }
        };
    }

    private void Update()
    {

    }

    private IEnumerator SpawnCenter(Vector2 reference, EnemyType enemyType)
    {
        Debug.Log("Spawning Center Formation");
        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Center]; i++)
        {
            Instantiate(prefab, reference, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnRandom(Vector2 reference, EnemyType enemyType)
    {
        Debug.Log("Spawning Random 'Formation'");
        float radius = spawnWidth / 2.0f;

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < _squadronSpawns[SpawnPattern.Random]; i++)
        {
            var spawnPos = reference;
            spawnPos.x += UnityEngine.Random.Range(-radius, radius);

            Instantiate(prefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnFlyingV(Vector2 reference, EnemyType enemyType, bool inverted)
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
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void Spawn(Squadron squadron)
    {
        Vector2 referenceVector;
        switch (squadron.SpawnZone)
        {
            case SpawnZone.Bottom:
                referenceVector = enemyBottomSpawnTransform.position;
                break;
            case SpawnZone.Top:
            case SpawnZone.Unknown:
            default:
                referenceVector = enemyTopSpawnTransform.position;
                break;
        }

        switch (squadron.SpawnPattern)
        {
            case SpawnPattern.Center:
                StartCoroutine(SpawnCenter(referenceVector, squadron.EnemyType));
                break;
            case SpawnPattern.Random:
                StartCoroutine(SpawnRandom(referenceVector, squadron.EnemyType));
                break;
            case SpawnPattern.FlyingV:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, false));
                break;
            case SpawnPattern.FlyingVInverted:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, true));
                break;
        }
    }
}
