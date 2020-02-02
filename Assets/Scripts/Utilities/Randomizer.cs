using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Randomizer
{
    public static SpawnZone GetSpawnZone()
    {
        return (SpawnZone)UnityEngine.Random.Range(1, 3);
    }

    public static EnemyType GetEnemyType()
    {
        return (EnemyType)UnityEngine.Random.Range(1, 2);
    }

    public static SpawnPattern GetSpawnPattern()
    {
        return (SpawnPattern)UnityEngine.Random.Range(1, 5);
    }
}
