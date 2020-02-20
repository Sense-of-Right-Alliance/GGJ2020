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

    protected override void UpdateTool()
    {
        //Debug.Log("Extinguisher updating!");
        if (on)
        {
            //Debug.Log("Extinguisher FIRING!");
            // Physics2D.OverlapCapsuleAll
            //float angle = Vector2.Angle(transform.up, m_MySecondVector);
            Vector2 start = transform.position;// + transform.up;
            Debug.DrawLine(start, transform.position + (transform.right * distance/2f));
            //Debug.DrawLine(start, transform.position + (transform.right * spread/2f));
            Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(start, new Vector2(distance, spread), CapsuleDirection2D.Vertical, 0); //OverlapCircleAll(transform.position, targetingRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Flame")
                {
                    //Debug.Log("Target Acquired!");
                    Flame f = hitColliders[i].GetComponent<Flame>();
                    f.Reduce(effectiveness * Time.deltaTime);
                }
                i++;
            }
        }
    }
}
