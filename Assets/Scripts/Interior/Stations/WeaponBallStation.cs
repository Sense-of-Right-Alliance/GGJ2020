using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponBallStation : Station
{
    protected override void ProcessResource(InteriorResource r)
    {
        Ship.AddEnergyBall(r.Value);
        base.ProcessResource(r);
    }

    protected override void AddScore()
    {
        ScoreManager.scoreManager.MainCannonCharged();
    }

    protected override void InitStation()
    {
        stationName = "Canon Station";
        description = "Stock to add Canon round to Pilot's inventory";
    }
}
