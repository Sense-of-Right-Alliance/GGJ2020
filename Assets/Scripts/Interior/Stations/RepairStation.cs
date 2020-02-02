using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RepairStation : Station
{
    protected override void ProcessResource(Resource r)
    {
        Debug.Log("Repair Station Processing!");
        // Todo: Repair ship HP

        Ship.RepairDamage(r.Value);
    }
}
