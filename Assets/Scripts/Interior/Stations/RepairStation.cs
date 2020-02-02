using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RepairStation : Station
{
    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Repair Station Processing!");
        // Todo: Repair ship HP

        Ship.RepairDamage(r.Value);
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
