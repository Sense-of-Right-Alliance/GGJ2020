using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorResource : PickupItem
{
    [SerializeField] int value = 1;

    public int Value { get { return value; } }

    public void Consume()
    {
        Debug.Log("Consuming Resource!");
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource) audioSource.Play(); // Note: won't play 'cause get's destroyed. TODO

        InteriorManager.interiorManager.ConsumeResource(gameObject);
    }
}
