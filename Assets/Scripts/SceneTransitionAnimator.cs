using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SceneTransitionAnimator : MonoBehaviour
{
    [SerializeField] GameObject transitionSwipePrefab;

    [SerializeField] Canvas canvas;

    public delegate void CallbackFunction(); // declare delegate type

    public CallbackFunction _callbackFunction; // to store the function

    private List<SceneTransitionAnimation> _animationList = new List<SceneTransitionAnimation>();

    private void Awake()
    {
        if (canvas == null) canvas = GameObject.FindObjectOfType<Canvas>();
    }

    private int animationIndex = -1;
    private void StartAnimation()
    {
        animationIndex = 0;
        _animationList[animationIndex].PlayAnimation(PlayNextAnimation);
    }

    private void PlayNextAnimation()
    {
        animationIndex++;
        if (animationIndex < _animationList.Count)
        {
            Debug.Log("SceneTransitionAnimator -> Playing Next Animation " + animationIndex.ToString());
            _animationList[animationIndex].PlayAnimation(PlayNextAnimation);
        }
        else
        {
            Debug.Log("SceneTransitionAnimator -> Animation complete!");
            // animation complete! Make callback
            animationIndex = -1;
            _callbackFunction();
        }
    }

    public void PlayMissionFailTransition(CallbackFunction callbackFunction)
    {
        _callbackFunction = callbackFunction;

        _animationList = new List<SceneTransitionAnimation>();
        _animationList.Add(new TransitionAnimationWait(1f));
        _animationList.Add(new TransitionAnimationSwipe(canvas, transitionSwipePrefab, 1f));

        StartAnimation();
    }

    public void PlayMissionCompletedTransition(CallbackFunction callbackFunction)
    {
        _callbackFunction = callbackFunction;

        _animationList = new List<SceneTransitionAnimation>();
        _animationList.Add(new TransitionAnimationWait(1f));
        _animationList.Add(new TransitionAnimationSwipe(canvas, transitionSwipePrefab, 1f));

        StartAnimation();
    }

    private void Update()
    {
        if (animationIndex >= 0 && animationIndex < _animationList.Count)
        {
            _animationList[animationIndex].Update(Time.deltaTime);
        }
    }
}
