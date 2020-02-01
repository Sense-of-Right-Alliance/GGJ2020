using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed = 5f;
    [SerializeField] int hitPoints = 2;

    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        rigidbody2D.AddForce(speed * Vector2.down);
    }

    public void TakeHit(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Ship>().takeHit(hitPoints); // deals damage in remaining hitpoints... 'cause yeah
            Destroy(gameObject);
        }
    }
}
