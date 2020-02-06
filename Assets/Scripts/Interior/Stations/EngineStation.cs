using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : DurationStation
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.BoostSpeedMult(r.Value * 2.5f, r.Value * Duration);
        base.ProcessResource(r);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.CrippleMovementMult(0.5f);
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.CrippleMovementMult(1f);
    }
}
