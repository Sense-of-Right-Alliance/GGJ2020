using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Squadron
{
    [SerializeField] SpawnPattern _pattern;
    public SpawnPattern SpawnPattern { get { return _pattern; } }

    [SerializeField] SpawnZone _zone;
    public SpawnZone SpawnZone { get { return _zone; } }

    [SerializeField] EnemyType _enemyType;
    public EnemyType EnemyType { get { return _enemyType; } }

    [SerializeField] int _count;
    public int Count { get { return _count; } private set { _count = value; } }

    [SerializeField] float _spawnDelay;
    public float SpawnDelay { get { return _spawnDelay; } private set { _spawnDelay = value; } }

    [SerializeField] bool _edgeBuffer;
    public bool EdgeBuffer { get { return _edgeBuffer; } private set { _edgeBuffer = value; } }

    public Squadron(EnemyType enemyType, SpawnPattern spawnPattern, SpawnZone spawnZone = SpawnZone.Top, int count = -1, float spawnDelay = -1, bool edgeBuffer = false)
    {
        _enemyType = enemyType;
        _pattern = spawnPattern;
        _zone = spawnZone;
        _count = count;
        _spawnDelay = spawnDelay;
        _edgeBuffer = edgeBuffer;
    }
}
