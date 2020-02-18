using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TransitionAnimationSwipe : SceneTransitionAnimation
{
    private Canvas _canvas;
    //private GameObject _swipePrefab;
    private float _duration;

    private GameObject swipe;
    private RectTransform r;

    private float _t = 0f;

    private float _startY = -700f;
    private float _endY = 0f;

    private bool _transitionOn = true;

    public TransitionAnimationSwipe(Canvas canvas, GameObject swipeObject, float duration, bool on)
    {
        _canvas = canvas;
        //_swipePrefab = swipePrefab;
        swipe = swipeObject;
        _duration = duration;
        _transitionOn = on;
    }

    public override void PlayAnimation(SceneTransitionAnimator.CallbackFunction callbackFunction)
    {
        base.PlayAnimation(callbackFunction);
        
        r = swipe.GetComponent<RectTransform>();
        swipe.GetComponent<Image>().fillOrigin = 0;
        RectTransform cr = _canvas.GetComponent<RectTransform>();

        _startY = 0f;
        _endY = 1f;

        if (_transitionOn)
        {
            swipe.GetComponent<Image>().fillOrigin = 1;
            
            float tmp = _startY;
            _startY = _endY;
            _endY = -1f*tmp;
        }

        swipe.GetComponent<Image>().fillAmount = _startY;

        _t = 0f;
    }

    public override void Update(float deltaTime)
    {
        _t += 2f * deltaTime;

        swipe.GetComponent<Image>().fillAmount = Mathf.Lerp(_startY, _endY, Mathf.Pow(_t, 2f));

        if (_t >= 1f)
        {
            animating = false;
            _callbackFunction();
        }
    }
}
