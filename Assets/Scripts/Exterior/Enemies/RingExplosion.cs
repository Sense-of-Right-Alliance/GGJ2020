using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingExplosion : MonoBehaviour
{
    [SerializeField] float damageRadius = 2f;
    [SerializeField] float damageDelay = 0.2f;

    private float t = 0f;
    private DamageDealer _damageDealer;

    private void Start()
    {
        _damageDealer = GetComponent<DamageDealer>();
    }

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
        int damage = 1;
        if (_damageDealer) damage = _damageDealer.GetDamage();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                hitColliders[i].GetComponent<ExteriorShip>().TakeHit(gameObject); // will take damage from DamageDealer component, and start problems from InteriorProblemMaker component
            }
            else if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].GetComponent<Enemy>().TakeHit(damage);
            }
            else if (hitColliders[i].tag == "Asteroid")
            {
                hitColliders[i].GetComponent<Asteroid>().TakeHit(damage);
            }
            i++;
        }
    }
}
