using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneTransitionAnimation
{
    protected SceneTransitionAnimator.CallbackFunction _callbackFunction;

    protected bool animating = false;

    public virtual void PlayAnimation(SceneTransitionAnimator.CallbackFunction callbackFunction)
    {
        _callbackFunction = callbackFunction;
        animating = true;
    }

    public virtual void Update(float deltaTime)
    {

    }
}
