using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class StatsUIPanel : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void DisplayStats(ScoreManager scoreManager)
    {
        transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + scoreManager.Score;

        transform.Find("HitsTakenText").GetComponent<TextMeshProUGUI>().text = scoreManager.HitsTaken + " Hits Taken";
        transform.Find("ResourcesCollectedText").GetComponent<TextMeshProUGUI>().text = scoreManager.ResourcesCollected + " Resources Collected";
        transform.Find("ResourcesConsumedText").GetComponent<TextMeshProUGUI>().text = scoreManager.ResourcesConsumed + " Resources Consumed";
        transform.Find("InteriorProblemsFixedText").GetComponent<TextMeshProUGUI>().text = scoreManager.InteriorProblemsFixed + " Problems Fixed";
        transform.Find("EnemiesDestroyedText").GetComponent<TextMeshProUGUI>().text = scoreManager.EnemiesDestroyed + " Enemies Destroyed";
    }
}
