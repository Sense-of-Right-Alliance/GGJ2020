using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;

    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Ship exteriorPlayer;
    [SerializeField] InteriorPlayer interiorPlayer;

    [SerializeField] int score = 0;
    public int Score { get { return score; } }

    [SerializeField] int pastMissionsScore = 0;
    public int CummulativeScore { get { return pastMissionsScore + score; } }

    private void Awake()
    {
        ScoreManager.scoreManager = this;
    }

    private void Start()
    {
        if (scoreText == null) scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();

        if (exteriorPlayer == null) exteriorPlayer = GameObject.FindObjectOfType<Ship>();
        exteriorPlayer.shipHitEvent.AddListener(PlayerShipHit);
        // todo subscrib to events

        if (interiorPlayer == null) interiorPlayer = GameObject.FindObjectOfType<InteriorPlayer>();
        // todo subscrib to events

        UpdatePastMissionsScore();

        UpdateScoreText();
    }

    private void UpdatePastMissionsScore()
    {
        int currentMission = PlayerPrefs.GetInt("mission_number");

        for (int i = 0; i < currentMission; i++)
        {
            pastMissionsScore += PlayerPrefs.GetInt("mission_" + i + "_score");
        }
    }

    public void PlayerShipHit() { AddScore(-100); }

    public void EnemyDestroyed(int scoreValue) { AddScore(scoreValue); }

    public void MainCannonCharged() { AddScore(500); }

    public void StationUsed() { AddScore(75); }

    public void FireExtinguished() { AddScore(100); }

    private void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = CummulativeScore.ToString(); // score.ToString();
    }
}
