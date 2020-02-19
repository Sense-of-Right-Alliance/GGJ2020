using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorResource : MonoBehaviour
{
    [SerializeField] int value = 1;

    public int Value { get { return value; } }
    public bool IsHeld { get { return isHeld; } }

    private bool isHeld = false;
    private InteriorPlayer holder;

    public void Hold(InteriorPlayer holder)
    {
        isHeld = true;
        this.holder = holder;
    }

    public void Drop()
    {
        isHeld = false;
        this.holder = null;
    }

    public void Consume()
    {
        Debug.Log("Consuming Resource!");
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource) audioSource.Play(); // Note: won't play 'cause get's destroyed. TODO

        InteriorManager.interiorManager.ConsumeResource(gameObject);
    }
}
