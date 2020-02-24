using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyShockwave : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] InteriorProblemOdds problemOdds;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Ship>().TakeHit(damage, problemOdds); 
        }
    }
}
