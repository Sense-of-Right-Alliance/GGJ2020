using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Station : MonoBehaviour
{
    [SerializeField] Ship exteriorShip;
    [SerializeField] bool activated = true;
    [SerializeField] int resourceRequirement = 1;
    [SerializeField] int resourceCount = 0;

    [SerializeField] GameObject resourcePipPrefab;

    protected ResourcePip[] resourcePips;

    protected bool Activated { get { return activated; } }
    protected int ResourceRequirement { get { return resourceRequirement; } }
    protected int ResourceCount { get { return resourceCount; } set { resourceCount = value; } }
    protected ResourcePip[] ResourcePips { get { return resourcePips; } }
    protected Ship Ship { get { return exteriorShip; }  }

    protected GameObject ResourcePipPrefab { get { return resourcePipPrefab; } }

    private void Awake()
    {
        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<Ship>();
    }

    private void Start()
    {
        Ship.shipHitEvent.AddListener(HandleShipHit);

        InitStation();

        InitPips();

        if (!activated) Deactivate();
    }

    protected virtual void InitStation()
    {

    }

    protected virtual void InitPips()
    {
        resourcePips = new ResourcePip[resourceRequirement];

        PositionPips(resourceRequirement);
    }

    protected void PositionPips(int numPips)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < numPips; i++)
        {
            Vector3 pipPos = transform.position;
            pipPos.y -= (spriteRenderer.sprite.bounds.size.y / 2f) + (spriteRenderer.sprite.bounds.size.y * 0.15f);

            float pipSpace = 0.16f;
            pipPos.x += (pipSpace * -Mathf.Floor(numPips / 2f)) + (pipSpace * i);

            GameObject pip = GameObject.Instantiate<GameObject>(resourcePipPrefab, pipPos, Quaternion.identity);
            pip.transform.SetParent(transform);

            resourcePips[i] = pip.GetComponent<ResourcePip>();
        }

        UpdateResourcePips();
    }

    protected virtual void UpdateResourcePips()
    {
        for (int i = 0; i < resourceRequirement; i++)
        {
            resourcePips[i].SetFull(i < resourceCount);
        }
    }

    protected virtual void HandleShipHit()
    {
        
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource" && overResource == collision.gameObject.GetComponent<InteriorResource>())
        {
            overResource = null;
            //Debug.Log("Station: resource over removed");
        }
    }

    protected bool resourceOver = false;
    protected InteriorResource overResource;
    protected virtual void HandleCollision(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            Debug.Log("Station: Collided with resource");
            overResource = collision.gameObject.GetComponent<InteriorResource>();
            if (activated && !overResource.IsHeld && TryCollectResource(overResource))
            {
                CollectResource(overResource);
            }
        }

        UpdateResourcePips();
    }
    */

    protected void CollectResource(InteriorResource r)
    {
        IncreaseResources(r);
        r.Consume();
        //overResource = null;

        UpdateResourcePips();
    }

    protected virtual void IncreaseResources(InteriorResource r)
    {
        resourceCount++;
        if (resourceCount >= resourceRequirement)
        {
            ProcessResource(r);
            resourceCount = 0;
        }
    }

    protected virtual void ProcessResource(InteriorResource r)
    {
        AddScore();
    }

    protected virtual void AddScore()
    {
        ScoreManager.scoreManager.StationUsed();
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

    public virtual void Reactivate()
    {
        activated = true;
    }

    private void Update()
    {
        /*
        if (overResource != null && activated && !overResource.IsHeld && TryCollectResource(overResource)) // handle case when player puts out fire while holding resource
        {
            //Debug.Log("Station: Found dropped resource!");
            CollectResource(overResource);
        }
        */

        StationUpdate();
    }

    // Interior Player will call this when it interacts with a station while holding a PickupItem
    public virtual void TryProcessItem(PickupItem item)
    {
        if (Activated)
        {
            InteriorResource ir = item.GetComponent<InteriorResource>();
            if (ir == null) ir = item.GetComponentInChildren<InteriorResource>();
            if (ir == null && item.transform.parent != null) ir = item.transform.parent.GetComponent<InteriorResource>();

            if (ir != null && TryCollectResource(ir))
            {
                CollectResource(ir);
            }
        }
    }

    protected virtual bool TryCollectResource(InteriorResource r)
    {
        return true;
    }

    protected virtual void StationUpdate()
    {
        // Cause Update cannot be inherited...
    }
}
