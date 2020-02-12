using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TransitionAnimationSwipe : SceneTransitionAnimation
{
    private Canvas _canvas;
    private GameObject _swipePrefab;
    private float _duration;

    private GameObject swipe;
    private RectTransform r;

    private float _t = 0f;

    private float _startY = -700f;
    private float _endY = 0f;

    private bool _transitionOn = true;

    public TransitionAnimationSwipe(Canvas canvas, GameObject swipePrefab, float duration, bool on)
    {
        _canvas = canvas;
        _swipePrefab = swipePrefab;
        _duration = duration;
        _transitionOn = on;
    }

    public override void PlayAnimation(SceneTransitionAnimator.CallbackFunction callbackFunction)
    {
        base.PlayAnimation(callbackFunction);

        swipe = GameObject.Instantiate(_swipePrefab);
        swipe.transform.SetParent(_canvas.transform);

        r = swipe.GetComponent<RectTransform>();

        if (_transitionOn)
        {
            float tmp = _startY;
            _startY = _endY;
            _endY = -1f*tmp;
        }

        Vector3 p = r.position;
        p.y = _startY;
        r.position = p;

        _t = 0f;
    }

    public override void Update(float deltaTime)
    {
        _t += 2f * deltaTime;

        Vector3 p = r.position;
        p.y = Mathf.Lerp(_startY, _endY, Mathf.Pow(_t,2f));// (700f / _duration) * deltaTime;
        r.position = p;

        if (_t >= 1f)
        {
            GameObject.Destroy(swipe);
            animating = false;
            _callbackFunction();
        }
    }
}
