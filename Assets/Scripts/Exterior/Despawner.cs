using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Despawner : MonoBehaviour
{
    [SerializeField] private string targetTag = "Enemy";

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(targetTag))
        {
            Destroy(collision.gameObject);
        }
    }
}
