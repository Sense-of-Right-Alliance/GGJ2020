using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float speed = 40f;
    [SerializeField] int damage = 1;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 newPos = transform.position;

        newPos.y += speed * Time.deltaTime;

        transform.position = newPos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("AmbushEnemy"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Enemy>().TakeHit(damage);
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Asteroid"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Asteroid>().TakeHit(damage);
            Destroy(gameObject);
        }
    }
}
