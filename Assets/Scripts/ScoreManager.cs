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

    // stats
    public int HitsTaken = 0;
    public int ResourcesCollected = 0;
    public int ResourcesConsumed = 0;
    public int InteriorProblemsFixed = 0;
    public int EnemiesDestroyed = 0; 
    public int AsteroidsDestroyed = 0; 

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

    public void PlayerShipHit() { AddScore(-100); HitsTaken++; }

    public void EnemyDestroyed(int scoreValue) { AddScore(scoreValue); EnemiesDestroyed++; }

    public void AsteroidDestroyed(int scoreValue) { AddScore(scoreValue); AsteroidsDestroyed++; }

    public void MainCannonCharged() { AddScore(500); }

    public void StationUsed() { AddScore(75); ResourcesConsumed++; }

    public void InteriorProblemFixed() { AddScore(100); InteriorProblemsFixed++; }

    public void ExteriorResourcePickedUp() { AddScore(50); ResourcesCollected++; }

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
