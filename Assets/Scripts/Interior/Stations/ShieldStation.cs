using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShieldStation : Station
{
    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Shields Station Processing!");

        //Ship.BoostFireRate(r.Value * 8f, r.Value * 5f);
        Ship.AddShields(r.Value);
    }

    protected override void InitPips()
    {
        resourcePips = new ResourcePip[Ship.MaxShields];

        PositionPips(Ship.MaxShields);
    }

    protected override void UpdateResourcePips()
    {
        for (int i = 0; i < ResourcePips.Length; i++)
        {
            ResourcePips[i].SetFull(i < Ship.Shields);
        }
    }

    protected override void HandleCollision(Collider2D collision)
    {
        if (Activated && collision.tag == "Interior Resource")
        {
            if (Ship.Shields < Ship.MaxShields)
            {
                ResourceCount++;
                if (ResourceCount >= ResourceRequirement)
                {
                    ProcessResource(collision.gameObject.GetComponent<Resource>());
                    ResourceCount = 0;
                }
            }

            Destroy(collision.gameObject);
        }

        UpdateResourcePips();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Ship.DisableShields();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        Ship.EnableShields();
    }

    protected override void HandleShipHit()
    {
        UpdateResourcePips();
    }
}
