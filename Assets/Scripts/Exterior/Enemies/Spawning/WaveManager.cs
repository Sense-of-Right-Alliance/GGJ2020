using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int waveNumber;
    public int WaveNumber => waveNumber;

    [SerializeField] bool randomWavesAfterScripted = false;
    [SerializeField] EnemyType endlessSingleEnemyWave = EnemyType.Unknown; // for debugging

    [SerializeField] TextMeshProUGUI waveText;

    [SerializeField] bool dontSpawn = false;

    public UnityEvent WavesCompletedEvent;
    public string MissionName { get { return GetComponent<MissionDetails>().Name; } }
    public bool WavesCompleted { get { return (waveNumber >= Waves.Count && endlessSingleEnemyWave == EnemyType.Unknown); } }

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
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.Center)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Column)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.FlyingV)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.Center)),
        }),
        new Wave(new List<WaveEvent>
        {
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.NovaSaucer, SpawnPattern.Center)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Column)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.NovaSaucer, SpawnPattern.Column)),
            WaveEvent.ShortDelay(),
            WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.FlyingV)),
        })

    };

    private void Awake()
    {
        _spawnManager = GetComponent<SpawnManager>();

        if (WavesCompletedEvent == null) WavesCompletedEvent = new UnityEvent();
    }

    private void InitializeWaves()
    {
        int currentMissionId = PlayerPrefs.GetInt("mission_number");

        MissionDetails missionDetails = GetComponent<MissionDetails>();
        if (missionDetails != null)
        {
            Waves = missionDetails.Waves;

            Debug.Log("Loading Mission " + missionDetails.Name + " : " + missionDetails.Description + " num waves = " + Waves.Count);
        }

        if (endlessSingleEnemyWave != EnemyType.Unknown)
        {
            Waves = new List<Wave>{
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(endlessSingleEnemyWave, SpawnPattern.Center)),
            }) };
        }
    }

    private void Update()
    {
        
    }

    private IEnumerator ProcessWave()
    {
        waveNumber += 1;

        if (waveText != null) waveText.text = "Wave " + waveNumber.ToString();

        Wave wave;
        if (waveNumber <= Waves.Count)
        {
            wave = Waves[waveNumber - 1];
        }
        else if (endlessSingleEnemyWave != EnemyType.Unknown)
        {
            wave = new Wave(new List<WaveEvent>
                {
                    WaveEvent.LongDelay(),
                    WaveEvent.SpawnSquadron(new Squadron(endlessSingleEnemyWave, SpawnPattern.Center)),
                });
        }
        else
        {
            WavesCompletedEvent.Invoke();

            if (randomWavesAfterScripted) Debug.Log("Wave " + waveNumber + " is not defined in WaveManager, so one will be generated randomly.");
            wave = GenerateRandomWave();
        }

        Debug.Log("ProcessingWave: " + wave.Name);

        if (waveNumber <= Waves.Count || randomWavesAfterScripted || endlessSingleEnemyWave != EnemyType.Unknown) // will end co-routine loop if out of waves and toggle not set
        {
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
    }

    public void StartWaves()
    {
        InitializeWaves();

        if (!dontSpawn) StartCoroutine(ProcessWave());
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
