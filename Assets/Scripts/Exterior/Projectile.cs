using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float speed = 40f;
    [SerializeField] bool indestructable = false;
    [SerializeField] int damage = 1;
    [SerializeField] float spin = 0;

    Vector2 direction = new Vector2(0, 1);
    List<GameObject> enemiesOver = new List<GameObject>(); // damage these enemies as long as we're over them, when indestructabl
    float timedDamage = 0.5f;
    float damageTimer = 0f;

    Rigidbody2D _rigidbody2D;
    //protected DamageDealer _damageDealer; // Not used... YET! Currently just an enemy thing.

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //_damageDealer = GetComponent<DamageDealer>();
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

        UpdateDamage();
    }

    private void UpdateDamage()
    {
        if (enemiesOver.Count > 0)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                for (int i = 0; i < enemiesOver.Count; i++)
                {
                    if (enemiesOver[i] == null)
                    {
                        enemiesOver.RemoveAt(i);
                        i--;
                        continue;
                    }

                    Debug.Log("Damaging enemy!");

                    Enemy e = enemiesOver[i].GetComponent<Enemy>();
                    if (e != null) e.TakeHit(damage);

                    Asteroid a = enemiesOver[i].GetComponent<Asteroid>();
                    if (a != null) a.TakeHit(damage);
                }
                damageTimer = timedDamage;
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        _rigidbody2D.AddForce(direction * speed);
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

            if (indestructable)
            {
                enemiesOver.Add(obj);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (obj.transform.CompareTag("Asteroid"))
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Asteroid>().TakeHit(damage);

            if (indestructable)
            {
                enemiesOver.Add(obj);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for (int i = 0; i < enemiesOver.Count; i++)
        {
            if (collision.gameObject == enemiesOver[i]) enemiesOver.Remove(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < enemiesOver.Count; i++)
        {
            if (collision.gameObject == enemiesOver[i]) enemiesOver.Remove(collision.gameObject);
        }
    }
}
