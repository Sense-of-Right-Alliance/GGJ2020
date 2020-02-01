using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponStation : Station
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    protected override void ProcessResource(Resource r)
    {
        Debug.Log("Weapon Station Processing!");
        Ship.BoostFireRate(r.Value * 3f, r.Value * 5f);
    }
}
