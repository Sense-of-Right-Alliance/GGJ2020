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
}
