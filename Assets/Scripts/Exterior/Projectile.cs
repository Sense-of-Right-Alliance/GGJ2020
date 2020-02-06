using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float speed = 40f;
    [SerializeField] int damage = 1;
    [SerializeField] bool indestructable = false;
    [SerializeField] float spin = 0;

    Vector2 direction = new Vector2(0, 1);

    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    public void Initialize(Vector2 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    private void Update()
    {
        if (spin > 0f) UpdateSpin();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        rigidbody2D.AddForce(direction * speed);
    }

    void UpdateSpin()
    {
        transform.Rotate(Vector3.forward * (spin * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void HandleCollision(GameObject obj)
    {
        if (obj.transform.CompareTag("Bounds") || obj.transform.CompareTag("Enemy Projectile"))
        {
            Destroy(gameObject);
        }
        else if (obj.transform.CompareTag("Enemy") || obj.transform.CompareTag("AmbushEnemy"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Enemy e = obj.GetComponent<Enemy>();
            if (e == null) Debug.Log("Projectile - collided object has no Enemy component. " + obj.name);
            else e.TakeHit(damage);

            if (!indestructable) Destroy(gameObject);
        }
        else if (obj.transform.CompareTag("Asteroid"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Asteroid>().TakeHit(damage);

            if (!indestructable) Destroy(gameObject);
        }
    }
}
