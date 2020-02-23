using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExtinguisherTool : Tool
{
    [SerializeField] GameObject particleEffect;
    [SerializeField] float distance = 1f;
    [SerializeField] float spread = 1f;
    [SerializeField] float effectiveness = 1f;

    ParticleSystem ps;

    protected override void UpdateTool()
    {
        if (on)
        {
            Transform t = transform.parent != null ? transform.parent : transform;

            Vector2 center = t.position + t.up * distance/2f;
            Debug.DrawLine(t.position, t.position + (t.up * distance));
            Debug.DrawLine(center, center + (Vector2)(t.right * spread/2f));
            Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(center, new Vector2(distance, spread), CapsuleDirection2D.Vertical, 0); //OverlapCircleAll(transform.position, targetingRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Flame")
                {
                    //don't extinguish past wall
                    bool hitWall = false;
                    RaycastHit2D[] hit = Physics2D.LinecastAll(t.position, hitColliders[i].transform.position);
                    for (int j = 0; j < hit.Length; j++)
                    { 
                        if (hit[j].transform.tag == "Wall")
                        {
                            hitWall = true;
                            break;
                        }
                    }

                    if (!hitWall)
                    {
                        Flame f = hitColliders[i].GetComponent<Flame>();
                        f.Reduce(effectiveness * Time.deltaTime);
                    }
                }
                i++;
            }
        }
    }

    protected override void UpdateVisuals()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        if (ps == null && transform.parent != null) ps = transform.parent.GetComponentInChildren<ParticleSystem>();
        if (ps == null && transform.parent != null) ps = transform.parent.GetComponent<ParticleSystem>();

        if (ps.isPlaying && !on)
        {
            ps.Stop();
        }
        else if (!ps.isPlaying && on)
        {
            ps.Play();
        }
    }
}
