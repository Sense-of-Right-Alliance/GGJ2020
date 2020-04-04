using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

// Takes damage on trigger collision from objects with DamageDealer components
public class Damageable : MonoBehaviour
{
    [SerializeField] int healthPoints = 10;

    public UnityEvent destroyedFromDamage = new UnityEvent(); // Other components can listen for this to do cool things

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            ProcessHit(damageDealer.GetDamage());
        }
    }

    private void ProcessHit(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
        {
            HandleDestroyed();
        }
    }

    private void HandleDestroyed()
    {
        destroyedFromDamage.Invoke();
        Destroy(gameObject);
    }
}
