using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlashSpriteAlpha : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float minAlpha = 0f;
    [SerializeField] float maxAlpha = 0.7f;

    private SpriteRenderer sr;

    private float t = 0f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        t += Time.deltaTime;

        Color newColor = sr.color;
        newColor.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Sin(t));

        if (t >= 6.283f) t = t - 6.283f;

        sr.color = newColor;
    }
}
