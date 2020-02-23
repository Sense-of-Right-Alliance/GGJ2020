using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickupItem : MonoBehaviour
{
    [SerializeField] float HoldOffsetMult = 1f;

    public bool IsHeld { get { return isHeld; } }

    private bool isHeld = false;
    private InteriorPlayer holder;

    private void Start()
    {
        
    }

    protected SpriteRenderer sr;
    protected SpriteRenderer GetSpriteRenderer()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null && transform.parent != null) sr = transform.parent.GetComponent<SpriteRenderer>();
        }

        return sr;
    }

    private void Update()
    {
        if (holder != null) UpdateItemPosition();

        UpdatePickupItem();
    }

    protected virtual void UpdatePickupItem()
    {

    }

    protected virtual void UpdateItemPosition()
    {
        Vector2 lookDir = holder.LookDirection.normalized;

        // Position
        Vector2 newPos = holder.transform.position;
        newPos += lookDir * (GetSpriteRenderer().sprite.bounds.size.x / 2 * HoldOffsetMult);

        if (transform.parent != null) transform.parent.position = newPos;
        else transform.position = newPos;

        // Rotation
       
        float rot_z = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public virtual void Pickup(InteriorPlayer holder)
    {
        isHeld = true;
        this.holder = holder;
    }

    public virtual void Drop()
    {
        isHeld = false;
        this.holder = null;
    }
}