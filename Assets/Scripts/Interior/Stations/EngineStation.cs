using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : DurationStation
{
    protected override void ProcessResource(Resource r)
    {
        base.ProcessResource(r);

        //Debug.Log("Engine Station Processing!");
        Ship.BoostSpeed(r.Value * 20f, r.Value * Duration);
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
