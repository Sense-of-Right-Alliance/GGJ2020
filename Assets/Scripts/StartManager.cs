using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] float logoAnimateSpeed = 5f;

    private int state = 0;

    private Vector2 startPos;
    private Vector2 endPos;
    private float t = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (state == 0) UpdateIdle();
        else if (state == 1) UpdateAnimate();
    }

    private void UpdateIdle()
    {
        if (Input.GetKeyDown(KeyCode.Space)
             || Input.GetButtonDown("A1") || Input.GetButtonDown("B1")
             || Input.GetButtonDown("A2") || Input.GetButtonDown("B2"))
        {
            startPos = logo.transform.position;

            endPos = startPos;
            endPos.y += 10;

            t = 0f;

            state++;
        }
    }

    private void UpdateAnimate()
    {
        t += Time.deltaTime;

        Vector2 newPos = logo.transform.position;
        newPos.y = Mathf.Lerp(startPos.y, endPos.y, Mathf.Pow(t,3));

        logo.transform.position = newPos;

        if (newPos.y >= 10)
        {
            SceneManager.LoadScene(1);
        }
    }
}
