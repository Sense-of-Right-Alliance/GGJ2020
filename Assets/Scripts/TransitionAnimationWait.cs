using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TransitionAnimationWait : SceneTransitionAnimation
{
    private float _duration;
    private float t = 0;
    public TransitionAnimationWait(float duration)
    {
        _duration = duration;
    }

    public override void PlayAnimation(SceneTransitionAnimator.CallbackFunction callbackFunction)
    {
        base.PlayAnimation(callbackFunction);

        t = 0;
    }

    public override void Update(float deltaTime)
    {
        t += deltaTime;

        if (t >= _duration)
        {
            animating = false;
            _callbackFunction();
        }
    }
}
