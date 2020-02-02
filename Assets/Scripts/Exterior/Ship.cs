using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] int MaxHitPoints = 5;

    [SerializeField] float speed = 3f;
    [SerializeField] float speedBoost = 0f;
    [SerializeField] float maxSpeed = 8f;

    [SerializeField] float fireRate = 3f; // shots per second
    [SerializeField] float fireRateBoost = 0f;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] ExteriorManager exteriorManager;

    [SerializeField] Transform projectileSpawnTransform;

    [SerializeField] Sprite[] damageSprites;
    [SerializeField] SpriteRenderer damageRenderer;

    [SerializeField] Vector2 velocity = Vector2.zero;

    private int currentHitPoints = 0;
    private float speedBoostTimer = 0f;

    private float fireTimer = 0f;
    
    private float fireRateBoostTimer = 0f;

    Rigidbody2D rigidbody2D;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        currentHitPoints = MaxHitPoints;
        UpdateDamageSprite();
        speedBoost = 0;
        fireRateBoost = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSpeedBoost();
        UpdateFireRateBoost();
        UpdateMovement();
        UpdateProjectile();
    }

    private void UpdateSpeedBoost()
    {
        speedBoostTimer -= Time.deltaTime;
        if (speedBoostTimer <= 0f)
        {
            speedBoostTimer = 0;
            speedBoost = 0;
        }
    }

    private void UpdateFireRateBoost()
    {
        fireRateBoostTimer -= Time.deltaTime;
        if (fireRateBoostTimer <= 0f)
        {
            fireRateBoostTimer = 0f;
            fireRateBoost = 0f;
        }
    }

    void UpdateMovement()
    {
        velocity *= 0.5f;

        float boostedSpeed = Mathf.Min(speed + speedBoost, maxSpeed);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal2") < 0)
        {
            velocity.x -= boostedSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal2") > 0)
        {
            velocity.x += boostedSpeed;// * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical2") < 0)
        {
            velocity.y += boostedSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical2") > 0)
        {
            velocity.y -= boostedSpeed;// * Time.deltaTime;
        }

        rigidbody2D.AddForce(velocity);
    }

    void UpdateProjectile()
    {
        fireTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("A2"))
        {
            if (fireTimer <= 0f)
            {
                fireTimer = 1f / (fireRate + fireRateBoost);
                Instantiate(projectilePrefab, projectileSpawnTransform.position, Quaternion.identity);
            }
        }
    }

    public void TakeHit(int damage)
    {
        currentHitPoints = Mathf.Max(currentHitPoints - damage, 0);

        if (currentHitPoints <= 0)
        {
            if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            exteriorManager.HandleShipDestroyed(this);
            Destroy(gameObject);
        }

        UpdateDamageSprite();
    }

    private void UpdateDamageSprite()
    {
        float percent = (currentHitPoints * 1f) / (MaxHitPoints * 1f);

        //Debug.Log("damage percent = " + percent + " index = " + Mathf.FloorToInt(damageSprites.Length * percent));

        if (percent >= 1f) damageRenderer.enabled = false;
        else
        {
            damageRenderer.enabled = true;
            damageRenderer.sprite = damageSprites[Mathf.FloorToInt(damageSprites.Length * percent)];
        }
    }

    public void RepairDamage(int amount)
    {
        currentHitPoints = Mathf.Min(currentHitPoints + amount, MaxHitPoints);

        UpdateDamageSprite();
    }

    public void BoostSpeed(float amount, float duration)
    {
        speedBoost += amount;

        speedBoostTimer += duration;

        Debug.Log("Ship Boosting Speed: Boost = " + speedBoost + " timer = " + speedBoostTimer);
    }

    public void BoostFireRate(float amount, float duration)
    {
        fireRateBoost = amount;
        fireRateBoostTimer += duration;
    }
}
