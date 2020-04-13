using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyShockwave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<ExteriorShip>().TakeHit(gameObject); // will take damage from DamageDealer component, and start problems from InteriorProblemMaker component
        }
    }
}
