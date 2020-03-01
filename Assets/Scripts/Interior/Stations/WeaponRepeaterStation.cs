using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponRepeaterStation : Station
{
    [SerializeField] float boostMultPerResource = 1f;
    [SerializeField] float duration = 30f;
    [SerializeField] int maxStacks = 3;
    [SerializeField] bool leftGun = true;
    [SerializeField] bool rightGun = true;

    protected float Duration { get { return duration; } }

    protected override void InitStation()
    {
        if (ResourceCount > 0)
        {
            // init with stacks already!
            for (int i = 0; i < ResourceCount; i++)
            {
                if (i < maxStacks) ProcessResource(null);
            }
        }
    }

    protected override void ProcessResource(InteriorResource r)
    {
        float value = r != null ? r.Value : 1;

        if (leftGun) Ship.AddBoostFireRateStackLeft(value * boostMultPerResource, value * Duration);
        if (rightGun) Ship.AddBoostFireRateStackRight(value * boostMultPerResource, value * Duration);


        if (r != null) base.ProcessResource(r);
    }

    protected override void IncreaseResources(InteriorResource r)
    {
        if (GetCurrentStacksOnShip() < maxStacks)
        {
            ResourceCount++;
            if (ResourceCount >= ResourceRequirement)
            {
                ProcessResource(r);
                ResourceCount = 0;
            }
        }
    }

    private int GetCurrentStacksOnShip()
    {
        return (leftGun ? Ship.NumFireRateBoostStacksLeft : Ship.NumFireRateBoostStacksRight);
    }

    protected override void InitPips()
    {
        resourcePips = new ResourcePip[maxStacks];

        PositionPips(maxStacks);
    }

    protected override void UpdateResourcePips()
    {
        for (int i = 0; i < ResourcePips.Length; i++)
        {
            ResourcePips[i].SetFull(i < GetCurrentStacksOnShip());
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        if (leftGun) Ship.DisableFiringLeft();
        if (rightGun) Ship.DisableFiringRight();
    }

    public override void Reactivate()
    {
        base.Reactivate();

        if (leftGun) Ship.EnableFiringLeft();
        if (rightGun) Ship.EnableFiringRight();
    }

    protected override void StationUpdate()
    {
        UpdateResourcePips();
    }
}
