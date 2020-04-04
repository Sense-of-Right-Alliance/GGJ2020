using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Damages objects with Damageable component
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 1;

    public int GetDamage() { return damage; }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
