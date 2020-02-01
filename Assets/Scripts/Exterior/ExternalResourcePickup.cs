using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExternalResourcePickup : MonoBehaviour
{
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
            InteriorManager interiorManager = GameObject.FindObjectOfType<InteriorManager>();

            if (interiorManager != null)
            {
                interiorManager.SpawnResource();
            }

            Destroy(gameObject);
        }
    }
}
