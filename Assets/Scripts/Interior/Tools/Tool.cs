using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : PickupItem
{
    protected bool on = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateTool();
    }

    protected virtual void UpdateTool()
    {

    }

    public virtual void ToggleOn()
    {
        on = !on;

        Debug.Log("Tool toggled to " + on.ToString());
    }

    public virtual void SetOn(bool on)
    {
        if (this.on != on)
        {
            Debug.Log("Toggling On to " + on);
            this.on = on;
        }
    }

    public override void Drop()
    {
        if (on) on = false;
    }
}
