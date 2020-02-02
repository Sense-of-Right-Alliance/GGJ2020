using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] int MaxHitPoints = 5;

    [SerializeField] float speed = 3f;
    [SerializeField] float speedBoost = 0f;
    [SerializeField] float maxSpeed = 10f;

    [SerializeField] float fireRate = 6f; // shots per second
    [SerializeField] float fireRateBoost = 0f;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] ExteriorManager exteriorManager;
    [SerializeField] InteriorManager interiorManager;

    [SerializeField] Transform projectileSpawnTransform;

    [SerializeField] Sprite[] damageSprites;
    [SerializeField] SpriteRenderer damageRenderer;

    [SerializeField] Vector2 velocity = Vector2.zero;

    private int currentHitPoints = 0;
    private float speedBoostTimer = 0f;
    private float crippledMovementSpeed = 0f;

    private bool firingEnabled = true;
    private float fireTimer = 0f;
    private float fireRateBoostTimer = 0f;

    Rigidbody2D rigidbody2D;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (exteriorManager == null) exteriorManager = GameObject.FindObjectOfType<ExteriorManager>();
        if (interiorManager == null) interiorManager = GameObject.FindObjectOfType<InteriorManager>();
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
        if (firingEnabled) UpdateProjectile();
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
        velocity = new Vector2();//*= 0.5f;

        float boostedSpeed = Mathf.Max(0,Mathf.Min(speed + speedBoost - crippledMovementSpeed, maxSpeed));

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x -= boostedSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += boostedSpeed;// * Time.deltaTime;
        } else
        {
            velocity.x = Input.GetAxis("Horizontal2") * boostedSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += boostedSpeed;// * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y -= boostedSpeed;// * Time.deltaTime;
        }
        else
        {
            velocity.y = Input.GetAxis("Vertical2") * boostedSpeed;
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

        interiorManager.HandleShipDamage();
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

        //Debug.Log("Ship Boosting Speed: Boost = " + speedBoost + " timer = " + speedBoostTimer);
    }

    public void BoostFireRate(float amount, float duration)
    {
        fireRateBoost = amount;
        fireRateBoostTimer += duration;
    }

    public void CrippleMovement(float amount)
    {
        crippledMovementSpeed = amount;
    }

    public void DisableFiring()
    {
        firingEnabled = false;
    }

    public void EnableFiring()
    {
        firingEnabled = true;
    }
}
