using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weldable : MonoBehaviour
{
    [SerializeField] float health = 10f;
    [SerializeField] GameObject explosionPrefab;

    protected SpriteRenderer sr;

    protected float rednessCooldown = 1f;
    protected float rednessTimer = 0f;

    private void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rednessTimer > 0f)
        {
            rednessTimer = Mathf.Max(0, rednessTimer - Time.deltaTime);
            Color c = Color.Lerp(Color.red, Color.white, 1f - (rednessTimer / rednessCooldown));
            sr.color = c;
        }
    }

    public void Weld(float power)
    {
        health -= power;
        if (health <= 0f) CompleteWeld();

        sr.color = Color.red;
        rednessTimer = rednessCooldown;
    }

    protected void CompleteWeld()
    {
        GetComponent<InteriorProblem>().HandleDestroyedOrRemoved();

        if (explosionPrefab != null)
        {
            GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Debug.Log("Creating Explosion! " + obj);
        }

        Destroy(gameObject);
    }
}
