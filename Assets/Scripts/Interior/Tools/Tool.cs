using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : PickupItem
{
    protected bool on = false;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    protected override void UpdatePickupItem()
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
            ToggleOn();
        }
    }

    public override void Drop()
    {
        base.Drop();

        if (on) on = false;
    }
}
