using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class StartManager : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] GameObject gorilla;
    [SerializeField] GameObject orangutan;

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] float logoAnimateSpeed = 5f;

    private int state = 0;

    private Vector2 startPos;
    private Vector2 endPos;
    private float t = 0f;

    private bool p1Ready = false;
    private bool p2Ready = false;

    private SpriteRenderer gorillaSprite;
    private SpriteRenderer orangutanSprite;

    private void Start()
    {
        gorillaSprite = gorilla.GetComponent<SpriteRenderer>();
        orangutanSprite = orangutan.GetComponent<SpriteRenderer>();

        gorillaSprite.color = Color.gray;
        orangutanSprite.color = Color.gray;

        int highScore = PlayerPrefs.GetInt("highscore");
        scoreText.text = "HS: " + highScore.ToString();

        PlayerPrefs.SetInt("mission_number", 0);
    }

    private void Update()
    {
        if (state == 0) UpdateIdle();
        else if (state == 1) UpdateAnimate();
    }

    private void UpdateIdle()
    {
        if (Input.GetButtonDown("A1") || Input.GetButtonDown("B1"))
        {
            p1Ready = true;
            gorillaSprite.color = Color.white;
        }

        if (Input.GetButtonDown("A2") || Input.GetButtonDown("B2"))
        {
            p2Ready = true;
            orangutanSprite.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Space) || (p1Ready && p2Ready))
        {
            gorillaSprite.color = Color.white;
            orangutanSprite.color = Color.white;
            
            startPos = logo.transform.position;

            endPos = startPos;
            endPos.y += 15;

            t = 0f;

            state++;
        }
    }

    bool logoDone = false;
    bool gorillaDone = false;
    bool orangutanDone = false;
    private void UpdateAnimate()
    {
        t += Time.deltaTime;

        Vector2 newPos = logo.transform.position;
        newPos.y = Mathf.Lerp(startPos.y, endPos.y, Mathf.Pow(t,3));
        if (newPos.y >= 15) { logoDone = true; }
        logo.transform.position = newPos;
        
        newPos = gorilla.transform.position;
        newPos.y = Mathf.Lerp(startPos.y, endPos.y, Mathf.Pow(t, 5));
        if (newPos.y >= 15) { gorillaDone = true; }
        gorilla.transform.position = newPos;

        newPos = orangutan.transform.position;
        newPos.y = Mathf.Lerp(startPos.y, endPos.y, Mathf.Pow(t, 5));
        if (newPos.y >= 15) { orangutanDone = true; }
        orangutan.transform.position = newPos;


        if (logoDone && gorillaDone && orangutanDone)
        {
            state = 2;
            GetComponent<SceneTransitionAnimator>().PlayStartGameTransition(LoadScene);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
