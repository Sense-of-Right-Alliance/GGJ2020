using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [SerializeField] SceneTransitionAnimator transitionAnimator;

    private void Awake()
    {
        GameManager.gameManager = this;

        if (transitionAnimator == null) transitionAnimator = GetComponent<SceneTransitionAnimator>();
    }

    public void CompleteMission()
    {
        Debug.Log("GameManager -> Mission Complete!");
        
        int currentMission = PlayerPrefs.GetInt("mission_number");

        PlayerPrefs.SetInt("mission_" + currentMission + "_score", ScoreManager.scoreManager.Score);

        currentMission++;
        if (currentMission >= ExteriorManager.exteriorManager.GetWaveManager().AllMissions.Length)
        {
            currentMission = 0;
            PlayerPrefs.SetInt("mission_number", currentMission);
            transitionAnimator.PlayMissionCompletedTransition(GoToStart);
        }
        else
        {
            PlayerPrefs.SetInt("mission_number", currentMission);
            transitionAnimator.PlayMissionCompletedTransition(GoToMission);
        }
        
    }

    public void FailMission()
    {
        Debug.Log("GameManager -> Mission Failed!");

        int highScore = PlayerPrefs.GetInt("highscore");
        if (ScoreManager.scoreManager.Score > highScore)
        {
            PlayerPrefs.SetInt("highscore", ScoreManager.scoreManager.Score);
        }

        transitionAnimator.PlayMissionFailTransition(GoToStart);
    }

    private void GoToStart()
    {
        SceneManager.LoadScene(0);
    }

    private void GoToMission()
    {
        SceneManager.LoadScene(1);
    }

    private void GoToStation()
    {
        // TODO
    }
}
