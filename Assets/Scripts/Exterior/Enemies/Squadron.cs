using System;
using System.Collections.Generic;
using System.Linq;

public class Squadron
{
    public SpawnPattern SpawnPattern { get; }

    public SpawnZone SpawnZone { get; }

    public EnemyType EnemyType { get; }

    public Squadron(EnemyType enemyType, SpawnPattern spawnPattern, SpawnZone spawnZone = SpawnZone.Top)
    {
        EnemyType = enemyType;
        SpawnPattern = spawnPattern;
        SpawnZone = spawnZone;
    }
}
