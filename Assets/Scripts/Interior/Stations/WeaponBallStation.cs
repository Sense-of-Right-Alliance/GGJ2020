using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponBallStation : Station
{
    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Weapon Station Processing!");

        Ship.FireEnergyBall(r.Value);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        //Ship.DisableFiring();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        //Ship.EnableFiring();
    }
}
