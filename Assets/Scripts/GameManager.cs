using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    enum GameState { Playing, Waiting, Confirming }

    [SerializeField] SceneTransitionAnimator transitionAnimator;
    [SerializeField] GameObject MissionFailedUI;
    [SerializeField] GameObject MissionCompletedUI;

    delegate void ConfirmCallback();
    ConfirmCallback confirmCallback;

    GameState state = GameState.Playing;

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
        
        currentMission = 0;
        PlayerPrefs.SetInt("mission_number", currentMission);
        transitionAnimator.PlayMissionCompletedTransition(ShowMissionCompletedUI);

        state = GameState.Waiting;

        /*
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
        */

    }

    private void Update()
    {
        if (state == GameState.Confirming)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A1") || Input.GetButtonDown("A2"))
            {
                if (confirmCallback != null) confirmCallback();
            }
        }
    }

    private void ShowMissionFailedUI()
    {
        MissionFailedUI.SetActive(true);
        MissionFailedUI.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score " + ScoreManager.scoreManager.Score;
        confirmCallback = GoToStart;
        state = GameState.Confirming;
    }

    private void ShowMissionCompletedUI()
    {
        MissionCompletedUI.SetActive(true);
        MissionFailedUI.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score " + ScoreManager.scoreManager.Score;
        confirmCallback = GoToStart;
        state = GameState.Confirming;
    }

    public void FailMission()
    {
        Debug.Log("GameManager -> Mission Failed!");

        int highScore = PlayerPrefs.GetInt("highscore");
        if (ScoreManager.scoreManager.Score > highScore)
        {
            PlayerPrefs.SetInt("highscore", ScoreManager.scoreManager.Score);
        }

        transitionAnimator.PlayMissionFailTransition(ShowMissionFailedUI);

        state = GameState.Waiting;
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
