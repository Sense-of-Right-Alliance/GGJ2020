using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDespawn : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
