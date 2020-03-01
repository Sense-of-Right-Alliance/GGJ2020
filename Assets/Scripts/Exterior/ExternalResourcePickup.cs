using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExternalResourcePickup : MonoBehaviour
{
    private float delayPickup = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (delayPickup > 0) delayPickup -= Time.deltaTime;

        if (delayPickup <= 0 && collidedOnDelay.Count > 0)
        {
            Pickup();
        }
    }

    List<GameObject> collidedOnDelay = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (delayPickup > 0)
            {
                collidedOnDelay.Add(collision.gameObject);
            }
            else
            {
                Pickup();
            }
        }
    }

    private void Pickup()
    {
        collidedOnDelay.Clear();

        InteriorManager interiorManager = GameObject.FindObjectOfType<InteriorManager>();

        if (interiorManager != null)
        {
            JettisonedObject jo = GetComponent<JettisonedObject>();
            if (jo != null)
            {
                interiorManager.ReclaimJettisonedObject(jo.InteriorObject);
            }
            else
            {
                interiorManager.SpawnResource();
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (delayPickup > 0) delayPickup = 0;

        if (collidedOnDelay.Count > 0)
        {
            collidedOnDelay.Remove(collision.gameObject);
        }
    }

    public void DelayPickup(float time)
    {
        delayPickup = time;
    }
}
