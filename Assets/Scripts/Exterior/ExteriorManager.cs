using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExteriorManager : MonoBehaviour
{
    private WaveManager _waveManager;

    private void Start()
    {
        _waveManager = GetComponent<WaveManager>();
        _waveManager.StartWaves();
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
}
