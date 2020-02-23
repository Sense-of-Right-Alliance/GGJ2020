using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SteamVent : MonoBehaviour
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

        Vector2 dir = ((t.position + (t.up * distance)) - t.position).normalized;

        //Debug.Log("pos = " + t.position.ToString() + " up *dist = " + (t.up * distance).ToString() + " normalized dir = " + dir.ToString());

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

                    float pDist = ((Vector2)hitColliders[i].transform.position - (Vector2)t.position).magnitude;
                    if (pDist < distance / 0.6f) p.DropItem();
                    // TODO: Scale push with how close they're to the vent
                    p.PushInDir(dir, push);
                }
                else
                {
                    Pushable p = hitColliders[i].GetComponent<Pushable>();
                    if (p != null)
                    {
                        float pDist = ((Vector2)hitColliders[i].transform.position - (Vector2)t.position).magnitude;
                        // TODO: Scale push with how close they're to the vent
                        p.PushInDir(dir, push);
                    }
                }
            }
            i++;
        }
    }
}
