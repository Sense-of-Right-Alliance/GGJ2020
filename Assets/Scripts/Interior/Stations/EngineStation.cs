using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : Station
{
    [SerializeField] float boostMultPerResource = 0.25f;
    [SerializeField] float duration = 30f;
    [SerializeField] int maxStacks = 3;

    protected float Duration { get { return duration; } }

    protected override void InitStation()
    {
        if (ResourceCount > 0)
        {
            // init with stacks already!
            for (int i = 0; i < ResourceCount; i++)
            {
                if (i < maxStacks) ProcessResource(null);
            }
        }

        stationName = "Engine Station";
        description = "Stock to increase ship speed and enable maneuvers";
    }

    protected override void ProcessResource(InteriorResource r)
    {
        float value = r != null ? r.Value : 1;
        Ship.AddBoostSpeedMultStack(value * boostMultPerResource, value * Duration);
        if (r != null) base.ProcessResource(r);
    }

    protected override void IncreaseResources(InteriorResource r)
    {
        if (Ship.NumEngineBoostStacks < maxStacks)
        {
            ResourceCount++;
            if (ResourceCount >= ResourceRequirement)
            {
                ProcessResource(r);
                ResourceCount = 0;
            }
        }
    }

    protected override void InitPips()
    {
        resourcePips = new ResourcePip[maxStacks];

        PositionPips(maxStacks);
    }

    protected override void UpdateResourcePips()
    {
        for (int i = 0; i < ResourcePips.Length; i++)
        {
            ResourcePips[i].SetFull(i < Ship.NumEngineBoostStacks);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.CrippleMovementMult(0.5f);
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.CrippleMovementMult(1f);
    }

    protected override void StationUpdate()
    {
        UpdateResourcePips();
    }
}
