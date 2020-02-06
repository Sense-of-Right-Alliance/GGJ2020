using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponRepeaterStation : DurationStation
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.BoostFireRate(r.Value * 8f, r.Value * Duration);
        base.ProcessResource(r);
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
