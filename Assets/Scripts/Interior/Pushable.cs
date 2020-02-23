using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pushable : MonoBehaviour
{
    [SerializeField] float basePushForce = 600f;

    Rigidbody2D rb;

    private void Start()
    {
       
    }

    private Rigidbody2D GetRigidbody2D()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        return rb;
    }

    private void Update()
    {

    }

    public void RandomPush(float forceMult = 1f)
    {
        Vector2 v = new Vector2(Random.Range(-1f, 1f) * basePushForce * forceMult, Random.Range(-1f, 1f) * basePushForce * forceMult);

        if (enabled) GetRigidbody2D().AddForce(v);
    }

    public void PushInDir(Vector2 dir, float force)
    {
        if (enabled) GetRigidbody2D().AddForce(dir * force);
    }
}
