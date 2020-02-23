using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExteriorManager : MonoBehaviour
{
    public static ExteriorManager exteriorManager; // Singletoooooooonnnnnnn!

    public SpawnManager GetSpawnManager() { return _spawnManager; }

    private WaveManager _waveManager;
    private SpawnManager _spawnManager;

    private void Awake()
    {
        ExteriorManager.exteriorManager = this;
    }

    private void Start()
    {
        _waveManager = GetComponent<WaveManager>();
        _waveManager.StartWaves();
        _waveManager.WavesCompletedEvent.AddListener(OnMissionWavesComplete);

        _spawnManager = GetComponent<SpawnManager>();
        _spawnManager.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);
    }

    private void Update()
    {

    }

    private void OnEnemyDestroyedOrRemoved(GameObject enemy)
    {
        CheckEndMission();
    }

    public void HandleShipDestroyed(Ship ship)
    {
        _waveManager.StopWaves();

        int highScore = PlayerPrefs.GetInt("highscore");
        if (ScoreManager.scoreManager.Score > highScore)
        {
            PlayerPrefs.SetInt("highscore", ScoreManager.scoreManager.Score);
        }

        GameManager.gameManager.FailMission();
    }
    
    public void OnMissionWavesComplete()
    {
        CheckEndMission();
    }

    public void CheckEndMission()
    {
        if (_spawnManager.NumEnemies <= 0 && _waveManager.WavesCompleted)
        {
            GameManager.gameManager.CompleteMission();
        }
    }
}
