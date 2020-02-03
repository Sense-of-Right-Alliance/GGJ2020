using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int waveNumber;
    public int WaveNumber => waveNumber;

    private SpawnManager _spawnManager;

    private List<Wave> Waves = new List<Wave>
    {
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingVInverted)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabRight, SpawnPattern.Column, SpawnZone.Right)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV, SpawnZone.Bottom)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabRight, SpawnPattern.Column, SpawnZone.Right)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV, SpawnZone.Bottom)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
            WaveEvent.MediumDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabRight, SpawnPattern.Column, SpawnZone.Right)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.LongDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV, SpawnZone.Bottom)),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Center, SpawnZone.Bottom)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
        })
        
    };

    private void Awake()
    {
        _spawnManager = GetComponent<SpawnManager>();
    }

    private void Update()
    {
        
    }

    private IEnumerator ProcessWave()
    {
        waveNumber += 1;

        Wave wave;
        if (waveNumber <= Waves.Count)
        {
            wave = Waves[waveNumber - 1];
        }
        else
        {
            Debug.Log("Wave " + waveNumber + " is not defined in WaveManager, so one will be generated randomly.");
            wave = GenerateRandomWave();
        }

        foreach (var waveEvent in wave.WaveEvents)
        {
            if (waveEvent.Squadron != null)
            {
                _spawnManager.Spawn(waveEvent.Squadron);
            }

            if (waveEvent.Duration > 0)
            {
                yield return new WaitForSeconds(waveEvent.Duration);
            }
        }

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ProcessWave());
    }

    public void StartWaves()
    {
        StartCoroutine(ProcessWave());
    }

    public void StopWaves()
    {
        StopAllCoroutines();
    }

    private Wave GenerateRandomWave()
    {
        var enemyType = Randomizer.GetEnemyType();
        var spawnPattern = Randomizer.GetSpawnPattern();
        var spawnZone = Randomizer.GetSpawnZone();

        var randomizedSquadron = new Squadron(enemyType, spawnPattern, spawnZone);

        return new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(randomizedSquadron),
            WaveEvent.MediumDelay(),
        });
    }
}
