using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;

    [SerializeField] Text scoreText;

    [SerializeField] Ship exteriorPlayer;
    [SerializeField] InteriorPlayer interiorPlayer;

    [SerializeField] int score = 0;

    private void Awake()
    {
        ScoreManager.scoreManager = this;
    }

    private void Start()
    {
        if (scoreText == null) scoreText = GameObject.Find("Score Text").GetComponent<Text>();

        if (exteriorPlayer == null) exteriorPlayer = GameObject.FindObjectOfType<Ship>();
        exteriorPlayer.shipHitEvent.AddListener(PlayerShipHit);
        // todo subscrib to events

        if (interiorPlayer == null) interiorPlayer = GameObject.FindObjectOfType<InteriorPlayer>();
        // todo subscrib to events



        UpdateScoreText();
    }

    private void PlayerShipHit()
    {
        AddScore(-100);
    }

    private void EnemyDestroyed(int hp)
    {
        AddScore(100 * hp);
    }

    private void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
