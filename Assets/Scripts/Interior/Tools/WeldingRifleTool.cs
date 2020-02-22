using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeldingRifleTool : Tool
{
    [SerializeField] float range = 2f;
    [SerializeField] GameObject weldingTipParticles;

    LineRenderer lr;

    protected override void UpdateTool()
    {
        if (on)
        {
            Transform t = transform.parent != null ? transform.parent : transform;
            
            Vector3 p1 = t.position;
            Vector3 p2 = p1 + t.up * range;

            Vector3 dir = (p2 - p1).normalized;
            
            // Check for a Wall.
            LayerMask mask = LayerMask.GetMask("InteriorBounds");

            // Check if a Wall is hit.
            RaycastHit2D hit = Physics2D.Raycast(t.position, dir, range, mask);

            

            if (hit.collider != null)
            {
                if (hit.transform.tag == "Wall")
                {
                    p2 = hit.point;
                    weldingTipParticles.SetActive(true);
                }
            }

            weldingTipParticles.transform.position = p2;
            float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            weldingTipParticles.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            if (lr != null)
            {
                lr.SetPosition(0, p1);
                lr.SetPosition(1, p2);
            }
        }
    }
    
    protected override void UpdateVisuals()
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        if (lr == null && transform.parent != null) lr = transform.parent.GetComponentInChildren<LineRenderer>();
        if (lr == null && transform.parent != null) lr = transform.parent.GetComponent<LineRenderer>();

        if (lr.enabled && !on)
        {
            lr.enabled = false;
            weldingTipParticles.SetActive(false);
        }
        else if (!lr.enabled && on)
        {
            lr.enabled = true;
        }
    }
}
