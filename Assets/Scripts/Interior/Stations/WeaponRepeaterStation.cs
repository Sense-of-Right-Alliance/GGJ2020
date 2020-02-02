using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponRepeaterStation : DurationStation
{
    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Weapon Station Processing!");
        Ship.BoostFireRate(r.Value * 8f, r.Value * Duration);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.DisableFiring();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.EnableFiring();
    }
}
