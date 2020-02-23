using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HullBreach : MonoBehaviour
{
    [SerializeField] float distance = 1f;
    [SerializeField] float spread = 1f;
    [SerializeField] float push = 100f;

    private void Update()
    {
        Transform t = transform;

        Vector2 center = t.position + t.up * distance / 2f;
        Debug.DrawLine(t.position, t.position + (t.up * distance));
        Debug.DrawLine(center, center + (Vector2)(t.right * spread / 2f));
        Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(center, new Vector2(spread, distance), CapsuleDirection2D.Vertical, 0); //OverlapCircleAll(transform.position, targetingRadius);

        Vector2 breachDir = (t.position - (t.position + (t.up * distance))).normalized;

        int i = 0;
        while (i < hitColliders.Length)
        {
            //don't extinguish past wall
            bool hitWall = false;
            RaycastHit2D[] hit = Physics2D.LinecastAll(t.position, hitColliders[i].transform.position);
            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].transform.tag == "Wall" || hit[j].transform.tag == "Debris")
                {
                    hitWall = true;
                    break;
                }
            }

            if (!hitWall)
            {
                if (hitColliders[i].tag == "Player")
                {
                    InteriorPlayer p = hitColliders[i].GetComponent<InteriorPlayer>();
                    Vector3 dir = (t.position - hitColliders[i].transform.position).normalized;
                    // TODO: Scale push with how close they're to the vent
                    p.PushInDir(dir, push);
                }
                else
                {
                    Pushable p = hitColliders[i].GetComponent<Pushable>();
                    if (p != null)
                    {
                        Vector3 dir = (t.position - hitColliders[i].transform.position).normalized;
                        // TODO: Scale push with how close they're to the vent
                        p.PushInDir(dir, push);
                    }
                }
            }
            i++;
        }

        for (int j = 0; j < heldItemsTouching.Count; j++)
        {
            if (!heldItemsTouching[j].IsHeld)
            {
                Vector3 dir = (transform.position - heldItemsTouching[j].transform.position).normalized;

                GameObject jObj = heldItemsTouching[j].gameObject;
                if (jObj.transform.parent != null && jObj.transform.parent.GetComponent<Pushable>() != null) jObj = jObj.transform.parent.gameObject;

                ExteriorManager.exteriorManager.GetSpawnManager().JettisonObject(jObj, dir);

                heldItemsTouching.Remove(heldItemsTouching[j]);
            }
        }
    }

    private List<PickupItem> heldItemsTouching = new List<PickupItem>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir;

        if (collision.gameObject.tag == "Player")
        {
            InteriorPlayer p = collision.gameObject.GetComponent<InteriorPlayer>();
            p.DropItem();

            dir = (transform.position - p.transform.position).normalized;
            
            ExteriorManager.exteriorManager.GetSpawnManager().JettisonObject(collision.gameObject, dir);
        }
        else
        {
            Pushable p = collision.gameObject.GetComponent<Pushable>();
            if (p != null)
            {
                PickupItem pi = collision.gameObject.GetComponent<PickupItem>();
                if (pi == null) pi = collision.gameObject.GetComponentInChildren<PickupItem>();
                if (pi == null && collision.gameObject.transform.parent != null) pi = collision.gameObject.transform.parent.GetComponent<PickupItem>();

                if (pi != null && pi.IsHeld) // don't jettison held items. Wait until the player is jettisoned haha!
                {
                    heldItemsTouching.Add(pi);
                }
                else
                {
                    dir = (transform.position - p.transform.position).normalized;
                    ExteriorManager.exteriorManager.GetSpawnManager().JettisonObject(collision.gameObject, dir);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Pushable p = collision.gameObject.GetComponent<Pushable>();
        if (p != null)
        {
            PickupItem pi = collision.gameObject.GetComponent<PickupItem>();
            if (pi == null) pi = collision.gameObject.GetComponentInChildren<PickupItem>();
            if (pi == null && collision.gameObject.transform.parent != null) pi = collision.gameObject.transform.parent.GetComponent<PickupItem>();

            if (pi != null && heldItemsTouching.Contains(pi))
            {
                heldItemsTouching.Remove(pi);
            }
        }
    }
}
