using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingExplosion : MonoBehaviour
{
    [SerializeField] float damageRadius = 2f;
    [SerializeField] float damageDelay = 0.2f;

    [SerializeField] InteriorProblemOdds problemOdds;

    private float t = 0f;

    private void Update()
    {
        Vector3 endRay = transform.position;
        endRay.x += damageRadius;
        Debug.DrawLine(transform.position, endRay);

        t += Time.deltaTime;

        if (t >= damageDelay)
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                hitColliders[i].GetComponent<Ship>().TakeHit(1, problemOdds);
            }
            else if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].GetComponent<Enemy>().TakeHit(1);
            }
            else if (hitColliders[i].tag == "Asteroid")
            {
                hitColliders[i].GetComponent<Asteroid>().TakeHit(1);
            }
            i++;
        }
    }
}
