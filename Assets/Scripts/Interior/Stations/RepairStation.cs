using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RepairStation : Station
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.RepairDamage(r.Value);
        base.ProcessResource(r);
    }

    protected override void InitPips()
    {
        resourcePips = new ResourcePip[Ship.MaxHitPoints];

        PositionPips(Ship.MaxHitPoints);
    }

    protected override void UpdateResourcePips()
    {
        for (int i = 0; i < ResourcePips.Length; i++)
        {
            ResourcePips[i].SetFull(i < Ship.HitPoints);
        }
    }

    protected override void HandleShipHit()
    {
        UpdateResourcePips();
    }
}
