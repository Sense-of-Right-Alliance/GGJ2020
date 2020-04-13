using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Station : MonoBehaviour
{
    [SerializeField] ExteriorShip exteriorShip;
    [SerializeField] bool activated = true;
    [SerializeField] int resourceRequirement = 1;
    [SerializeField] int resourceCount = 0;

    [SerializeField] GameObject resourcePipPrefab;

    [SerializeField] AudioClip addResourceSFX;
    [SerializeField] AudioClip doEffectSFX;
    [SerializeField] AudioClip disableSFX;

    [SerializeField] protected GameObject TooltipPrefab;

    protected ResourcePip[] resourcePips;

    protected bool Activated { get { return activated; } }
    protected int ResourceRequirement { get { return resourceRequirement; } }
    protected int ResourceCount { get { return resourceCount; } set { resourceCount = value; } }
    protected ResourcePip[] ResourcePips { get { return resourcePips; } }
    protected ExteriorShip Ship { get { return exteriorShip; }  }

    protected GameObject ResourcePipPrefab { get { return resourcePipPrefab; } }
    
    protected Tooltip tooltip;

    protected string stationName = "Station";
    protected string description = "Consumes resources to provide effect";

    private AudioSource aSource;

    private void Awake()
    {
        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<ExteriorShip>();

        aSource = GetComponent<AudioSource>();

        if (addResourceSFX == null) addResourceSFX = Resources.Load("phaserUp4") as AudioClip;
        if (doEffectSFX == null) doEffectSFX = Resources.Load("powerUp1") as AudioClip;
        if (disableSFX == null) disableSFX = Resources.Load("phaserDown3") as AudioClip;
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


    public void ShowTooltip()
    { 
        if (tooltip == null)
        {
            tooltip = Instantiate(TooltipPrefab, transform).GetComponent<Tooltip>();
            tooltip.Text.text = stationName + "\n" + description;
            //tooltip.GetComponent<RectTransform>().rect.height = tooltip.Text.text.
            //tooltip.gameObject.transform.SetParent(this.transform);
            //tooltip.gameObject.GetComponent<RectTransform>().position = Vector2.zero;
        }

        tooltip.gameObject.SetActive(true);
        //tooltip
    }

    public void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.gameObject.SetActive(false);
        }
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
            GameObject pip = GameObject.Instantiate<GameObject>(resourcePipPrefab, Vector2.zero, Quaternion.identity);
            
            // y
            Vector3 pipPos = transform.position;
            pipPos.y -= (spriteRenderer.sprite.bounds.size.y / 2f) + (spriteRenderer.sprite.bounds.size.y * 0.2f);
            // x
            float pipWidth = pip.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float pipBuffer = pipWidth * 0.3f;
            float pipBarLength = (pipWidth * (float)numPips) + (pipBuffer * (float)(numPips - 1));
            float halfLength = pipBarLength / 2.0f;
            pipPos.x = (pipWidth/2.0f) + pipPos.x - halfLength + ((pipWidth+pipBuffer) * i);
            
            pip.transform.position = pipPos;
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

    protected virtual void HandleShipHit(GameObject hittingObject)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") ShowTooltip();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") HideTooltip();
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

            aSource.PlayOneShot(doEffectSFX, 0.5f);
        }
        else
        {
            aSource.PlayOneShot(addResourceSFX, 0.5f);
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

        aSource.PlayOneShot(disableSFX, 0.2f);
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
