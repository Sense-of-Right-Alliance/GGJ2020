using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupItem : MonoBehaviour
{
    public bool IsHeld { get { return isHeld; } }

    private bool isHeld = false;
    private InteriorPlayer holder;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public virtual void Pickup(InteriorPlayer holder)
    {
        isHeld = true;
        this.holder = holder;
    }

    public virtual void Drop()
    {
        isHeld = false;
        this.holder = null;
    }
}