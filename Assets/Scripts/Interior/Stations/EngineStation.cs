using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : DurationStation
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.BoostSpeed(r.Value * 20f, r.Value * Duration);
        base.ProcessResource(r);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.CrippleMovement(15f);
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.CrippleMovement(0f);
    }
}
