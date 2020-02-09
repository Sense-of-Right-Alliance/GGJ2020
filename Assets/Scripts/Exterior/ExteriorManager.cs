using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExteriorManager : MonoBehaviour
{
    public static ExteriorManager exteriorManager; // Singletoooooooonnnnnnn!

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

        _spawnManager = GetComponent<SpawnManager>();
    }

    private void Update()
    {

    }

    public void HandleShipDestroyed(Ship ship)
    {
        Debug.Log("ExteriorManager -> Game Over!");

        _waveManager.StopWaves();

        int highScore = PlayerPrefs.GetInt("highscore");
        if (ScoreManager.scoreManager.Score > highScore)
        {
            PlayerPrefs.SetInt("highscore", ScoreManager.scoreManager.Score);
        }

        SceneManager.LoadScene(0);
    }

    public void HandleMissionWavesComplete()
    {
        Debug.Log("ExteriorManager -> Mission Waves Completed!");
        CheckEndMession();
    }

    public void CheckEndMession()
    {
        if (_spawnManager.NumEnemies <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
