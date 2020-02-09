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

    public Squadron(EnemyType enemyType, SpawnPattern spawnPattern, SpawnZone spawnZone = SpawnZone.Top)
    {
        _enemyType = enemyType;
        _pattern = spawnPattern;
        _zone = spawnZone;
    }
}
