using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShieldStation : Station
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.AddShields(r.Value);
        UpdateResourcePips();
        base.ProcessResource(r);
    }

    protected override void InitStation()
    {
        stationName = "Shield Station";
        description = "Stock to add shield that protects the ship";
    }

    protected override void InitPips()
    {
        resourcePips = new ResourcePip[Ship.MaxShields];

        PositionPips(Ship.MaxShields);
    }

    protected override void UpdateResourcePips()
    {
        for (int i = 0; i < ResourcePips.Length; i++)
        {
            ResourcePips[i].SetFull(i < Ship.Shields);
        }
    }

    protected override void IncreaseResources(InteriorResource r)
    {
        if (Ship.Shields < Ship.MaxShields)
        {
            ResourceCount++;
            if (ResourceCount >= ResourceRequirement)
            {
                ProcessResource(r);
                ResourceCount = 0;
            }
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.DisableShields();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.EnableShields();
    }

    protected override void HandleShipHit(GameObject hittingObject)
    {
        UpdateResourcePips();
    }
}
