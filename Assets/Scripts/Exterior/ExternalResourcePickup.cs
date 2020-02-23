using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExternalResourcePickup : MonoBehaviour
{
    private bool delayUntilExit = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !delayUntilExit)
        {
            InteriorManager interiorManager = GameObject.FindObjectOfType<InteriorManager>();

            if (interiorManager != null)
            {
                JettisonedObject jo = GetComponent<JettisonedObject>();
                if (jo != null)
                {
                    interiorManager.ReclaimJettisonedObject(jo.InteriorObject);
                } else
                {
                    interiorManager.SpawnResource();
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (delayUntilExit) delayUntilExit = false;
    }

    public void DelayCollisionUntilExit()
    {
        delayUntilExit = true;
    }
}
