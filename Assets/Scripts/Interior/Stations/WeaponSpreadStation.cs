using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSpreadStation : DurationStation
{
    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Weapon SPREAD Station Processing!");

        Ship.AddWeaponSpread(r.Value * Duration);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.DisableSpread();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.EnableSpread();
    }
}
