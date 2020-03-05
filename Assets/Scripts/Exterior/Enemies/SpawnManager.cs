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
    [SerializeField] GameObject homingCorvettePrefab;
    [SerializeField] GameObject novaSaucerPrefab;
    [SerializeField] GameObject mediumShrapnelAsteroidPrefab;
    [SerializeField] GameObject shockwavePrefab;
    [SerializeField] GameObject bulletAsteroidPrefab;
    [SerializeField] GameObject enemyCanonPrefab;
    [SerializeField] GameObject resourceAsteroidPrefab;
    [SerializeField] float spawnDelay = 0.5f; // seconds between each ship spawn in squadron
    [SerializeField] Transform enemyTopSpawnTransform;
    [SerializeField] Transform enemyBottomSpawnTransform;
    [SerializeField] Transform enemyLeftSpawnTransform;
    [SerializeField] Transform enemyRightSpawnTransform;
    [SerializeField] Transform asteroidTopSpawnTransform;
    [SerializeField] Transform asteroidBottomSpawnTransform;
    [SerializeField] float spawnWidth = 10f;
    [SerializeField] GameObject resourcePrefab;

    public UnityGameObjectEvent EnemyDestroyedOrRemovedEvent; // event for managers to listen to, like Exterior for tracking game end
    public int NumEnemies { get { return _enemySpawns.Count; } }

    private readonly Dictionary<SpawnPattern, int> _squadronSpawns = new Dictionary<SpawnPattern, int>
    {
        { SpawnPattern.Unknown, 0 },
        { SpawnPattern.Center, 1 },
        { SpawnPattern.FlyingV, 5 },
        { SpawnPattern.FlyingVInverted, 5 },
        { SpawnPattern.Column, 6 },
        { SpawnPattern.DoubleColumn, 6 },
        { SpawnPattern.Random, 6 },
        { SpawnPattern.JostledRow, 12 },
    };

    private Dictionary<EnemyType, GameObject> _enemyPrefabs;
    private List<GameObject> _enemySpawns = new List<GameObject>();

    private GameObject playerShip;
    private GameObject GetPlayerShip()
    {
        if (playerShip == null)
        {
            playerShip = GameObject.FindObjectOfType<Ship>().gameObject;
        }

        return playerShip;
    }

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
            { EnemyType.HomingCorvet, homingCorvettePrefab },
            { EnemyType.NovaSaucer, novaSaucerPrefab },
            { EnemyType.MediumShrapnelAsteroid,  mediumShrapnelAsteroidPrefab },
            { EnemyType.Shockwave, shockwavePrefab },
            { EnemyType.BulletAsteroid, bulletAsteroidPrefab },
            { EnemyType.Canon, enemyCanonPrefab },
            { EnemyType.ResourceAsteroid, resourceAsteroidPrefab },
        };

        if (EnemyDestroyedOrRemovedEvent == null) EnemyDestroyedOrRemovedEvent = new UnityGameObjectEvent();
    }

    private void Update()
    {
        Vector3 left = enemyTopSpawnTransform.position - Vector3.right * spawnWidth / 2;
        Vector3 right = enemyTopSpawnTransform.position + Vector3.right * spawnWidth / 2;
        Debug.DrawLine(left, right);
    }

    private void OnEnemyDestroyedOrRemoved(GameObject enemy)
    {
        _enemySpawns.Remove(enemy);

        Enemy eComp = enemy.GetComponent<Enemy>();
        Asteroid aComp = enemy.GetComponent<Asteroid>();

        if (eComp) eComp.EnemyDestroyedOrRemovedEvent.RemoveListener(OnEnemyDestroyedOrRemoved);
        else if (aComp) aComp.EnemyDestroyedOrRemovedEvent.RemoveListener(OnEnemyDestroyedOrRemoved);

        //Debug.Log("Removed! Num Spawns = " + _enemySpawns.Count);
        EnemyDestroyedOrRemovedEvent.Invoke(enemy);
    }

    private void SpawnEnemy(GameObject prefab, Vector2 spawnPos, Quaternion rotation, string tagName)
    {
        var enemy = Instantiate(prefab, spawnPos, rotation);
        if (transform.parent != null) enemy.transform.SetParent(transform.parent);
        enemy.tag = tagName;
        _enemySpawns.Add(enemy);

        Enemy eComp = enemy.GetComponent<Enemy>();
        Asteroid aComp = enemy.GetComponent<Asteroid>();
        if (eComp) eComp.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);
        else if (aComp) aComp.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);

        //Debug.Log("Spawned! Num Spawns = " + _enemySpawns.Count);
    }

    private IEnumerator SpawnColumn(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, int count, float spawnDelay)
    {
        Debug.Log("Spawning Column Formation");

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < (count == -1 ? _squadronSpawns[SpawnPattern.Column] : count); i++)
        {
            var spawnPos = reference;

            SpawnEnemy(prefab, spawnPos, rotation, tagName);

            yield return new WaitForSeconds(spawnDelay == -1 ? this.spawnDelay : spawnDelay * 2.5f);
        }
    }

    private IEnumerator SpawnCenter(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, int count, float spawnDelay)
    {
        Debug.Log("Spawning Center Formation");
        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < (count == -1 ? _squadronSpawns[SpawnPattern.Center] : count); i++)
        {
            SpawnEnemy(prefab, reference, rotation, tagName);

            yield return new WaitForSeconds(spawnDelay == -1 ? this.spawnDelay : spawnDelay);
        }
    }

    private IEnumerator SpawnRandom(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, int count, float spawnDelay)
    {
        Debug.Log("Spawning Random 'Formation'");
        float radius = spawnWidth / 2.0f;

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < (count == -1 ? _squadronSpawns[SpawnPattern.Random] : count); i++)
        {
            var spawnPos = reference;
            spawnPos.x += UnityEngine.Random.Range(-radius, radius);

            SpawnEnemy(prefab, spawnPos, rotation, tagName);

            yield return new WaitForSeconds(spawnDelay == -1 ? this.spawnDelay : spawnDelay);
        }
    }

    private IEnumerator SpawnJostledRow(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, int count, float spawnDelay)
    {
        Debug.Log("Spawning Jostled Row");
        float radius = spawnWidth / 2.0f;
        var numberOfSpawns = (count == -1 ? _squadronSpawns[SpawnPattern.JostledRow] : count);

        float spacing = 0f;
        if (numberOfSpawns != 1)
        {
            spacing = spawnWidth / (numberOfSpawns - 1);
        }

        float jostleRange = 1f;

        var spawnLocations = new List<List<Vector2>>(numberOfSpawns);
        for (var i = 0; i < numberOfSpawns; i++)
        {
            spawnLocations.Add(new List<Vector2>());

            var position = new Vector2(reference.x - radius + spacing * i, reference.y);
            position.x += UnityEngine.Random.Range(-jostleRange, jostleRange);
            position.y += UnityEngine.Random.Range(0, jostleRange*2);
            spawnLocations[i].Add(position);
        }

        var prefab = _enemyPrefabs[enemyType];

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            for (int j = 0; j < spawnLocations[i].Count; j++)
            {
                var spawnPos = spawnLocations[i][j];
                SpawnEnemy(prefab, spawnPos, rotation, tagName);
            }

            yield return new WaitForSeconds(0f);
        }
    }

    private IEnumerator SpawnFlyingV(Vector2 reference, EnemyType enemyType, Quaternion rotation, string tagName, bool inverted, int count, float spawnDelay)
    {
        Debug.Log("Spawning Flying V Formation " + (inverted ? "(inverted)" : "(normal)"));
        float radius = spawnWidth / 2.0f;
        var numberOfSpawns = (count == -1 ? _squadronSpawns[SpawnPattern.FlyingV] : count);

        float spacing = 0f;
        if (numberOfSpawns != 1)
        {
            spacing = spawnWidth / ((numberOfSpawns - 1)*2);
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

            yield return new WaitForSeconds(spawnDelay == -1 ? this.spawnDelay : spawnDelay);
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

        if (squadron.EnemyType == EnemyType.HugeAsteroid
            || squadron.EnemyType == EnemyType.MediumAsteroid 
            || squadron.EnemyType == EnemyType.MediumShrapnelAsteroid
            || squadron.EnemyType == EnemyType.BulletAsteroid
            || squadron.EnemyType == EnemyType.ResourceAsteroid)
        {
            tagName = "Asteroid";
        }

        switch (squadron.SpawnPattern)
        {
            case SpawnPattern.Center:
                StartCoroutine(SpawnCenter(referenceVector, squadron.EnemyType, rotation, tagName, squadron.Count, squadron.SpawnDelay));
                break;
            case SpawnPattern.Random:
                StartCoroutine(SpawnRandom(referenceVector, squadron.EnemyType, rotation, tagName, squadron.Count, squadron.SpawnDelay));
                break;
            case SpawnPattern.FlyingV:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tagName, false, squadron.Count, squadron.SpawnDelay));
                break;
            case SpawnPattern.FlyingVInverted:
                StartCoroutine(SpawnFlyingV(referenceVector, squadron.EnemyType, rotation, tagName, true, squadron.Count, squadron.SpawnDelay));
                break;
            case SpawnPattern.Column:
                StartCoroutine(SpawnColumn(referenceVector, squadron.EnemyType, rotation, tagName, squadron.Count, squadron.SpawnDelay));
                break;
            case SpawnPattern.JostledRow:
                StartCoroutine(SpawnJostledRow(referenceVector, squadron.EnemyType, rotation, tagName, squadron.Count, squadron.SpawnDelay));
                break;
        }
    }

    public void JettisonObject(GameObject interiorObject, Vector3 dir)
    {
        // just get left or right
        Vector3 spawnDir = Vector3.zero;
        if (dir.x < 0) spawnDir = new Vector3(-1, 0, 0);
        if (dir.x > 0) spawnDir = new Vector3(1, 0, 0);
        
        GameObject playerShip = GetPlayerShip();

        if (playerShip != null)
        {
            interiorObject.SetActive(false);

            GameObject r = SpawnResource(playerShip.transform.position + spawnDir * 0.5f);
            r.GetComponent<SpaceDrift>().SetDrifDirection(dir);
            r.GetComponent<ExternalResourcePickup>().DelayPickup(1.5f);
            r.AddComponent<JettisonedObject>();
            r.GetComponent<JettisonedObject>().SetInteriorObject(interiorObject);
        }
    }

    public GameObject SpawnResource(Vector3 pos)
    {
        GameObject r = Instantiate(resourcePrefab, pos, Quaternion.identity);

        return r;
    }
}
