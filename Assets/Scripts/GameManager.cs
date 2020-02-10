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
        transitionAnimator.PlayMissionCompletedTransition(GoToStart);
    }

    public void FailMission()
    {
        Debug.Log("GameManager -> Mission Failed!");
        transitionAnimator.PlayMissionFailTransition(GoToStart);
    }

    private void GoToStart()
    {
        SceneManager.LoadScene(0);
    }

    private void GoToStation()
    {
        // TODO
    }
}
