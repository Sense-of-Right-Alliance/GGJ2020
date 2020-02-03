using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DurationStation : Station
{
    [SerializeField] int maxStacks = 3;
    [SerializeField] float duration = 30f;

    protected float Duration { get { return duration; } }

    List<float> pipDurationStacks = new List<float>();

    protected override void ProcessResource(Resource r)
    {
        //Debug.Log("Weapon SPREAD Station Processing!");

        //Ship.AddWeaponSpread(r.Value * duration);
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
            ResourcePips[i].SetFull(i < pipDurationStacks.Count);
        }
    }

    protected override void HandleCollision(Collider2D collision)
    {
        if (Activated && collision.tag == "Interior Resource")
        {
            if (pipDurationStacks.Count < maxStacks)
            {
                ResourceCount++;
                if (ResourceCount >= ResourceRequirement)
                {
                    ProcessResource(collision.gameObject.GetComponent<Resource>());
                    pipDurationStacks.Add(duration);
                    ResourceCount = 0;
                }
            }

            Destroy(collision.gameObject);
        }

        UpdateResourcePips();
    }

    private void Update()
    {
        UpdateDurationPips();
    }

    private void UpdateDurationPips()
    {
        if (pipDurationStacks.Count > 0)
        {
            for (int i = pipDurationStacks.Count - 1; i >= 0; i--)
            {
                pipDurationStacks[i] -= Time.deltaTime;
                if (pipDurationStacks[i] <= 0f)
                {
                    pipDurationStacks.RemoveAt(i);
                    i++;
                    if (pipDurationStacks.Count == 0) break;
                }
            }
        }
        UpdateResourcePips();
    }
}
