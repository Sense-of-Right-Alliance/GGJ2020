using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EngineStation : Station
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    protected override void ProcessResource(Resource r)
    {
        Debug.Log("Engine Station Processing!");
        Ship.BoostSpeed(r.Value * 3f, r.Value * 6f);
    }
}
