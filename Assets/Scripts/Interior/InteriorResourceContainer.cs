using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorResourceContainer : MonoBehaviour
{
    [SerializeField] InteriorResource interiorResource;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void RandomPush()
    {
        Vector2 v = new Vector2(Random.Range(-1f, 1f) * 600f, Random.Range(-1f, 1f) * 600f);

        rb.AddForce(v);
    }
}
