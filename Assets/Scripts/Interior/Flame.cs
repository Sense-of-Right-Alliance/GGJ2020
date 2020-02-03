using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flame : MonoBehaviour
{
    [SerializeField] float healthPoints = 2.5f; // time it takes to smother out the fire

    [SerializeField] bool smotheringFire = false;

    float startingHealth = 1f;

    Station station;

    bool ignited = false;

    private void Start()
    {
        startingHealth = healthPoints;
    }

    public void Ignite(Station station)
    {
        ignited = true;
        this.station = station;
        station.Deactivate();
    }

    private void Update()
    {
        if (ignited == true && smotheringFire == true)
        {
            healthPoints -= Time.deltaTime;

            float percent = (healthPoints / startingHealth) * 0.7f + 0.3f;
            Vector2 newScale = transform.localScale;
            newScale.x = percent;
            newScale.y = percent;

            transform.localScale = newScale;

            if (healthPoints <= 0f)
            {
                Extinguish();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        smotheringFire = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        smotheringFire = false;
    }

    private void Extinguish()
    {
        station.Reactivate();

        ScoreManager.scoreManager.FireExtinguished();
        
        Destroy(gameObject);
    }
}
