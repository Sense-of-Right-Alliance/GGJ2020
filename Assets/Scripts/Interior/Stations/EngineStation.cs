using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : Station
{

    protected override void ProcessResource(Resource r)
    {
        Debug.Log("Engine Station Processing!");
        Ship.BoostSpeed(r.Value * 3f, r.Value * 6f);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.CrippleMovement(3f);
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.CrippleMovement(0f);
    }
}
