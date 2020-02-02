using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Despawner : MonoBehaviour
{
    [SerializeField] private string[] targetTags = { "Enemy" };

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < targetTags.Length; i++)
        {
            if (collision.transform.CompareTag(targetTags[i]))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
