using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour
{
    public UnityEvent shipHitEvent;

    [SerializeField] int maxHitPoints = 5;
    public int MaxHitPoints { get { return maxHitPoints; } }

    [SerializeField] float speed = 3f;
    [SerializeField] float speedBoost = 0f;
    [SerializeField] float maxSpeed = 60f;

    [SerializeField] float fireRate = 6f; // shots per second
    [SerializeField] float fireRateBoost = 0f;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject energyBallPrefab;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] ExteriorManager exteriorManager;
    [SerializeField] InteriorManager interiorManager;

    [SerializeField] Transform projectileSpawnTransform;

    [SerializeField] Sprite[] damageSprites;
    [SerializeField] SpriteRenderer damageRenderer;

    [SerializeField] Sprite[] shieldsSprites;
    [SerializeField] SpriteRenderer shieldsRenderer;

    [SerializeField] Vector2 velocity = Vector2.zero;

    private int currentHitPoints = 0;
    public int HitPoints { get { return currentHitPoints; } }
    private float speedBoostTimer = 0f;
    private float crippledMovementSpeed = 0f;

    private bool firingEnabled = true;
    private float fireTimer = 0f;
    private float fireRateBoostTimer = 0f;

    Rigidbody2D rigidbody2D;
    AudioSource audioSource;

    List<float> spreadStacks = new List<float>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (exteriorManager == null) exteriorManager = GameObject.FindObjectOfType<ExteriorManager>();
        if (interiorManager == null) interiorManager = GameObject.FindObjectOfType<InteriorManager>();

        if (shipHitEvent == null) shipHitEvent = new UnityEvent();

        currentHitPoints = maxHitPoints;
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateDamageSprite();
        UpdateShieldsSprite();
        speedBoost = 0;
        fireRateBoost = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSpeedBoost();
        UpdateFireRateBoost();
        UpdateWeaponSpread();
        UpdateMovement();
        if (firingEnabled) UpdateProjectile();
    }

    private void UpdateSpeedBoost()
    {
        if (speedBoost > 0f)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0f)
            {
                speedBoostTimer = 0f;
                speedBoost = 0f;
            }
        }
    }

    private void UpdateFireRateBoost()
    {
        if (fireRateBoost > 0f)
        {
            fireRateBoostTimer -= Time.deltaTime;
            if (fireRateBoostTimer <= 0f)
            {
                fireRateBoostTimer = 0f;
                fireRateBoost = 0f;
            }
        }
    }

    private void UpdateWeaponSpread()
    {
        if (spreadStacks.Count > 0 && spreadEnabled)
        {
            for (int i = spreadStacks.Count-1; i >= 0; i--)
            {
                spreadStacks[i] -= Time.deltaTime;
                if (spreadStacks[i] <= 0f)
                {
                    spreadStacks.RemoveAt(i);
                    i++;
                    if (spreadStacks.Count == 0) break;
                }
            }
        }        
    }

    void UpdateMovement()
    {
        velocity = new Vector2();//*= 0.5f;

        float boostedSpeed = Mathf.Max(0, Mathf.Min(speed + speedBoost - crippledMovementSpeed, maxSpeed));// * Time.deltaTime;

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
                audioSource.Play();

                fireTimer = 1f / (fireRate + fireRateBoost);
                Instantiate(projectilePrefab, projectileSpawnTransform.position, Quaternion.identity);

                if (spreadEnabled) // spread fire
                {
                    Vector2 newPos;
                    float spreadDist = 0.5f;
                    for (int i = 0; i < spreadStacks.Count; i++)
                    {
                        newPos = projectileSpawnTransform.position;
                        newPos.x -= spreadDist * (i + 1);
                        Instantiate(projectilePrefab, newPos, Quaternion.identity);
                        newPos = projectileSpawnTransform.position;
                        newPos.x += spreadDist * (i + 1);
                        Instantiate(projectilePrefab, newPos, Quaternion.identity);
                    }
                }
            }
        }
    }

    public void TakeHit(int damage)
    {
        if (shields > 0)
        {
            shields = Mathf.Max(shields - 1, 0); // could change to damage, if wanted that. But currently just negates a hit
            UpdateShieldsSprite();
        }
        else
        {
            currentHitPoints = Mathf.Max(currentHitPoints - damage, 0);
            UpdateDamageSprite();

            if (currentHitPoints <= 0)
            {
                if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                exteriorManager.HandleShipDestroyed(this);
                Destroy(gameObject);
            }
        }

        shipHitEvent.Invoke();
        interiorManager.HandleShipDamage();
    }

    private void UpdateDamageSprite()
    {
        float percent = (currentHitPoints * 1f) / (maxHitPoints * 1f);

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
        currentHitPoints = Mathf.Min(currentHitPoints + amount, maxHitPoints);

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

    public void DisableFiring() { firingEnabled = false; }
    public void EnableFiring() { firingEnabled = true; }
    
    public void AddWeaponSpread(float duration)
    {
        spreadStacks.Add(duration);
    }
    private bool spreadEnabled = true;
    public void DisableSpread() { spreadEnabled = false; }
    public void EnableSpread() { spreadEnabled = true; }

    public void FireEnergyBall(float size)
    {
        if (energyBallPrefab != null)
        {
            Instantiate(energyBallPrefab, transform.position, Quaternion.identity);
        }
    }

    [SerializeField] int shields = 0;
    public int Shields { get { return shields; } }
    public int MaxShields { get { return shieldsSprites.Length; } }

    private bool shieldsEnabled = true;
    public void AddShields(int amount)
    {
        shields = Mathf.Min(shields +amount, shieldsSprites.Length);

        UpdateShieldsSprite();
    }

    private void UpdateShieldsSprite()
    {
        if (shields <= 0 || !shieldsEnabled) shieldsRenderer.enabled = false;
        else
        {
            shieldsRenderer.enabled = true;
            shieldsRenderer.sprite = shieldsSprites[shields-1];
        }
    }

    public void DisableShields() { shieldsEnabled = false; UpdateShieldsSprite(); }
    public void EnableShields() { shieldsEnabled = true; UpdateShieldsSprite(); }
}
