using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] int hitPoints = 5;
    [SerializeField] float horizontalSpeed = 5f;
    [SerializeField] float verticalSpeed = 5f;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] ExteriorManager exteriorManager;

    [SerializeField] Transform projectileSpawnTransform;

    [SerializeField] Vector2 velocity = Vector2.zero;

    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMovement();
        UpdateProjectile();
    }

    void UpdateMovement()
    {
        velocity *= 0.5f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x -= horizontalSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += horizontalSpeed;// * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += horizontalSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y -= horizontalSpeed;// * Time.deltaTime;
        }

        /*
        Vector3 newPos = transform.position;
        newPos.x += velocity.x;
        newPos.y += velocity.y;
        */

        //transform.position = newPos;

        rigidbody2D.AddForce(velocity);
    }

    void UpdateProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, projectileSpawnTransform.position, Quaternion.identity);
        }
    }

    public void takeHit(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            exteriorManager.HandleShipDestroyed(this);
            Destroy(gameObject);
        }
    }

    public void repairDamage(int damage)
    {
        hitPoints += damage;
    }
}
