using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponSpreadStation : DurationStation
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.AddWeaponSpread(r.Value * Duration);
        base.ProcessResource(r);
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
