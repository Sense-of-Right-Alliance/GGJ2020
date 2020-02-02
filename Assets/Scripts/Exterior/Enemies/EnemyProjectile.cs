using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float wobbleAmount = 0f;

    private Vector2 _direction;

    // Start is called before the first frame update
    private void Start()
    {
        _direction = -transform.up;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 newPos = transform.position;

        newPos.y += _direction.y * speed * Time.deltaTime;
        newPos.x += _direction.x * speed * Time.deltaTime + UnityEngine.Random.Range(-wobbleAmount, wobbleAmount);

        transform.position = newPos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bounds"))
        {
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Player"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Ship>().TakeHit(damage);
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
