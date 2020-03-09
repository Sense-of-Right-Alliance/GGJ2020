using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExteriorResourceDrop : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;

    [SerializeField] float dropChanceOnDestroy;
    [SerializeField] float dropChanceOnHit;
    [SerializeField] int dropCount;

    private void Start()
    {
        /*
        if (dropChanceOnDestroy >= 1f || dropChanceOnHit >= 1f)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr) sr.color = Color.yellow;
        }
        */
    }

    private void Update()
    {
        
    }

    public void HandleHit()
    {
        if (Random.value < dropChanceOnHit)
        {
            for (int i = 0; i < dropCount; i++)
            {
                SpawnResource();
            }
        }
    }

    public void HandleDestroy()
    {
        if (Random.value < dropChanceOnDestroy)
        {
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        for (int i = 0; i < dropCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f), 0f);
            
            GameObject obj = Instantiate(resourcePrefab, pos, transform.rotation);
            obj.transform.Rotate(Vector3.forward * Random.Range(0f, 360f));
        }
    }
}
