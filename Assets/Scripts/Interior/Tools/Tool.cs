using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tool : PickupItem
{
    protected bool on = false;

    private AudioSource aSource;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        Init();
    }

    protected virtual void Init()
    {
        UpdateVisuals();
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

        if (on) aSource.Play();
        else aSource.Stop();

        UpdateVisuals();

        //Debug.Log("Tool toggled to " + on.ToString());
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

        UpdateVisuals();
    }

    protected virtual void UpdateVisuals()
    {

    }

    private void OnEnable()
    {
        UpdateVisuals();
        //Debug.Log("PrintOnEnable: script was enabled");
    }
}
