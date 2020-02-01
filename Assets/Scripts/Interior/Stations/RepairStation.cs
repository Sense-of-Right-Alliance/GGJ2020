using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RepairStation : Station
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    protected override void ProcessResource(Resource r)
    {
        Debug.Log("Repair Station Processing!");
        // Todo: Repair ship HP

        Ship.RepairDamage(r.Value);
    }
}
