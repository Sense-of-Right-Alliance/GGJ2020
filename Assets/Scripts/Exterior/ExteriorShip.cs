using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExteriorShip : MonoBehaviour
{
    public UnityEvent shipHitEvent;

    [SerializeField] Camera exteriorBoundsCamera;
    [SerializeField] float boundsPadding = 1f;

    [SerializeField] int maxHitPoints = 5;
    public int MaxHitPoints { get { return maxHitPoints; } }

    [SerializeField] bool invincible = false;

    [SerializeField] float speed = 50f;
    [SerializeField] float speedBoostMult = 1f;
    [SerializeField] float maxSpeed = 10000f;

    [SerializeField] float fireRateLeft = 3f; // shots per second
    [SerializeField] float fireRateBoostLeft = 0f;
    [SerializeField] float fireRateRight = 3f; // shots per second
    [SerializeField] float fireRateBoostRight = 0f;
    private bool firingEnabledLeft = true;
    private bool firingEnabledRight = true;
    private float fireTimerLeft = 0f;
    private float fireTimerRight = 0f;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject energyBallPrefab;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] ExteriorManager exteriorManager;
    [SerializeField] InteriorManager interiorManager;

    [SerializeField] Transform projSpawnFront;
    [SerializeField] Transform projSpawnLeft;
    [SerializeField] Transform projSpawnRight;

    [SerializeField] Sprite[] damageSprites;
    [SerializeField] SpriteRenderer damageRenderer;

    [SerializeField] Sprite[] shieldsSprites;
    [SerializeField] SpriteRenderer shieldsRenderer;

    [SerializeField] Vector2 velocity = Vector2.zero;

    [SerializeField] PlayerID playerID = PlayerID.Player2;

    [SerializeField] GameObject engineFlameEffect;
    [SerializeField] GameObject engineSmokeEffect;
    [SerializeField] GameObject engineTrailEffect;

    [SerializeField] private int currentHitPoints = 5;
    public int HitPoints { get { return currentHitPoints; } }
    public float HitPointPercent { get { return (float)currentHitPoints / (float)MaxHitPoints; } }

    private float speedBoostTimer = 0f;
    [SerializeField] float crippledSpeedMult = 1f;
    
    [SerializeField] List<float> spreadStacks = new List<float>();
    [SerializeField] List<ShipBonusStack> engineBoostStacks = new List<ShipBonusStack>();
    [SerializeField] List<ShipBonusStack> fireRateBoostStacksLeft = new List<ShipBonusStack>();
    [SerializeField] List<ShipBonusStack> fireRateBoostStacksRight = new List<ShipBonusStack>();

    [SerializeField] int numEnergyBalls = 0;

    public int NumFireSpreadStacks { get { return spreadStacks.Count; } }
    public int NumEngineBoostStacks { get { return engineBoostStacks.Count; } }
    public int NumFireRateBoostStacksLeft { get { return fireRateBoostStacksLeft.Count; } }
    public int NumFireRateBoostStacksRight { get { return fireRateBoostStacksRight.Count; } }

    Rigidbody2D _rigidbody2D;
    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;

    // bounds variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (exteriorManager == null) exteriorManager = GameObject.FindObjectOfType<ExteriorManager>();
        if (interiorManager == null) interiorManager = GameObject.FindObjectOfType<InteriorManager>();
        if (exteriorBoundsCamera == null) exteriorBoundsCamera = GameObject.Find("ExteriorCamera").GetComponent<Camera>();

        if (shipHitEvent == null) shipHitEvent = new UnityEvent();

        //currentHitPoints = maxHitPoints; // disabled, so can set starting health in editor.

        // calc bounds

        if (exteriorBoundsCamera != null)
        {
            xMin = exteriorBoundsCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + boundsPadding;
            xMax = exteriorBoundsCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - boundsPadding;

            yMin = exteriorBoundsCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + boundsPadding;
            yMax = exteriorBoundsCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - boundsPadding;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        UpdateDamageSprite();
        UpdateShieldsSprite();
        //speedBoostMult = 1;
        //fireRateBoost = 0f;

        int savedPlayerID = PlayerPrefs.GetInt("Pilot", -1);
        if (savedPlayerID == 0) playerID = PlayerID.Player1;
        else if (savedPlayerID == 1) playerID = PlayerID.Player2;

        engineFlameEffect.SetActive(crippledSpeedMult >= 1);
        engineSmokeEffect.SetActive(crippledSpeedMult < 1);
        engineTrailEffect.SetActive(speedBoostMult > 1f);

        UpdateEnergyBallUI();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateSpeedBoost();
        UpdateFireRateBoost();
        UpdateWeaponSpread();

        UpdateProjectile();
        UpdateManeuvers();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateSpeedBoost()
    {
        speedBoostMult = 1;

        if (engineStacksDrainMult > 1f) Debug.Log("Stack drain " + engineStacksDrainMult);

        // update stack timers
        for (int i = 0; i < engineBoostStacks.Count; i++)
        {
            if (i == 0) engineBoostStacks[i].Duration -= Time.deltaTime * engineStacksDrainMult; // only remove from the oldest stack, as this is like a fuel reserve
            if (engineBoostStacks[i].Duration <= 0)
            {
                engineBoostStacks.RemoveAt(i--);
            } else
            {
                speedBoostMult += engineBoostStacks[i].Amount;
            }
        }

        speedBoostMult = Mathf.Max(1, speedBoostMult);

        engineTrailEffect.SetActive(speedBoostMult > 1f);
    }

    private void UpdateFireRateBoost()
    {
        fireRateBoostLeft = 0;
             
        // update stack timers
        for (int i = 0; i < fireRateBoostStacksLeft.Count; i++)
        {
            if (i == 0) fireRateBoostStacksLeft[i].Duration -= Time.deltaTime * fireRateStackDrainLeft; // only remove from the oldest stack, as this is like an ammo reserve
            if (fireRateBoostStacksLeft[i].Duration <= 0)
            {
                fireRateBoostStacksLeft.RemoveAt(i--);
            }
            else
            {
                fireRateBoostLeft += fireRateBoostStacksLeft[i].Amount;
            }
        }

        fireRateBoostRight = 0;

        // update stack timers
        for (int i = 0; i < fireRateBoostStacksRight.Count; i++)
        {
            if (i == 0) fireRateBoostStacksRight[i].Duration -= Time.deltaTime * fireRateStackDrainRight;
            if (fireRateBoostStacksRight[i].Duration <= 0)
            {
                fireRateBoostStacksRight.RemoveAt(i--);
            }
            else
            {
                fireRateBoostRight += fireRateBoostStacksRight[i].Amount;
            }
        }

        fireRateStackDrainLeft = 0; // resets each frame.
        fireRateStackDrainRight = 0; // resets each frame.
    }

    private void UpdateWeaponSpread()
    {
        if (spreadStacks.Count > 0 && spreadEnabled)
        {
            for (int i = spreadStacks.Count - 1; i >= 0; i--)
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

    private void UpdateMovement()
    {
        velocity = new Vector2();

        float boostedSpeed = Mathf.Max(0, Mathf.Min((speed * speedBoostMult) * crippledSpeedMult, maxSpeed));

        if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.LeftArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.A)))
        {
            velocity.x -= boostedSpeed;
        }
        else if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.RightArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.D)))
        {
            velocity.x += boostedSpeed;
        }
        else
        {
            velocity.x = (playerID == PlayerID.Player1 ? Input.GetAxis("Horizontal1") : Input.GetAxis("Horizontal2")) * boostedSpeed;
        }


        if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.UpArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.W)))
        {
            velocity.y += boostedSpeed;
        }
        else if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.DownArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.S)))
        {
            velocity.y -= boostedSpeed;
        }
        else
        {
            velocity.y = (playerID == PlayerID.Player1 ? Input.GetAxis("Vertical1") : Input.GetAxis("Vertical2")) * boostedSpeed;
        }

        // Correct if at bounds...
        if (exteriorBoundsCamera != null) // means we've got mins and maxs defined in start
        {
            if (velocity.x < 0 && transform.position.x < xMin) velocity.x = 0;
            if (velocity.x > 0 && transform.position.x > xMax) velocity.x = 0;
            if (velocity.y < 0 && transform.position.y < yMin) velocity.y = 0;
            if (velocity.y > 0 && transform.position.y > yMax) velocity.y = 0;
        }

        _rigidbody2D.AddForce(velocity);

        // TODO: Apply opposite velocity to interior player and objects
        InteriorManager.interiorManager.AddInertiaVelocityToObjects(-velocity);
    }

    private float fireRateStackDrainLeft = 0f; // won't drain fire rate stacks unless are firing.
    private float fireRateStackDrainRight = 0f; // won't drain fire rate stacks unless are firing.

    private void UpdateProjectile()
    {
        if (fireTimerLeft > 0) fireTimerLeft -= Time.deltaTime;
        if (fireTimerRight > 0) fireTimerRight -= Time.deltaTime;

        bool fireLeft = (playerID == PlayerID.Player1 && (Input.GetKey(KeyCode.G) || (Input.GetAxis("TriggerLeft1") > 0)) || (playerID == PlayerID.Player2 && (Input.GetKey(KeyCode.Space) || (Input.GetAxis("TriggerLeft2") > 0))));
        bool fireRight = (playerID == PlayerID.Player1 && (Input.GetKey(KeyCode.G) || (Input.GetAxis("TriggerRight1") > 0)) || (playerID == PlayerID.Player2 && (Input.GetKey(KeyCode.Space) || (Input.GetAxis("TriggerRight2") > 0))));

        if (fireLeft && fireTimerLeft <= 0f && firingEnabledLeft)
        {
            fireTimerLeft = 1f / (fireRateLeft + fireRateBoostLeft);
            FireProjectile(projSpawnLeft);
            fireRateStackDrainLeft = fireRateLeft + fireRateBoostLeft;
        }

        if (fireRight && fireTimerRight <= 0f && firingEnabledRight)
        {
            fireTimerRight = 1f / (fireRateRight + fireRateBoostRight);
            FireProjectile(projSpawnRight);
            fireRateStackDrainRight = fireRateRight + fireRateBoostRight;
        }


        /* Disabled spread for the moment, as we added left and right controlls
        if (spreadEnabled) // spread fire
        {
            Vector2 newPos;
            float spreadDist = 0.5f;
            for (int i = 0; i < spreadStacks.Count; i++)
            {
                newPos = projSpawnFront.position;
                newPos.x -= spreadDist * (i + 1);
                Instantiate(projectilePrefab, newPos, Quaternion.identity);
                newPos = projSpawnFront.position;
                newPos.x += spreadDist * (i + 1);
                Instantiate(projectilePrefab, newPos, Quaternion.identity);
            }
        }
        */

        if ((playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("X1")) || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.V) || Input.GetButtonDown("X2")))))
        {
            FireEnergyBall(1f); // checks if have enough in inventory
        }
    }

    private void UpdateManeuvers()
    {
        if (barrelRollTimer > 0f)
        {
            barrelRollTimer -= Time.deltaTime;

            _spriteRenderer.color = Color.Lerp(Color.gray, Color.black, Mathf.Sin(barrelRollTimer));

            if (barrelRollTimer <= 0f)
            {
                _spriteRenderer.color = Color.white;

                StopBarrelRoll();
            }
        }
        else
        {
            if (crippledSpeedMult >= 1f && engineBoostStacks.Count > 0 && (playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("B1")) || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("B2")))))
            {
                StartBarrelRoll(0.5f);
            }
        }
    }

    private float engineStacksDrainMult = 1f;
    private float barrelRollTimer = 0f;
    private void StartBarrelRoll(float duration)
    {
        barrelRollTimer = duration;
        GetComponent<Collider2D>().enabled = false;

        engineStacksDrainMult = 3f;
    }

    private void StopBarrelRoll()
    {
        GetComponent<Collider2D>().enabled = true;

        engineStacksDrainMult = 1f;
    }

    private void FireProjectile(Transform t)
    {
        _audioSource.Play();
        Instantiate(projectilePrefab, t.position, t.rotation);
    }

    public void TakeHit(int damage, InteriorProblemOdds problemOdds = null)
    {
        if (!invincible && barrelRollTimer <= 0f)
        { 
            if (shields > 0 && ShieldsEnabled)
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
        }

        shipHitEvent.Invoke();
        interiorManager.HandleShipDamage(problemOdds);
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

    public void AddBoostSpeedMultStack(float amount, float duration)
    {
        engineBoostStacks.Add(new ShipBonusStack(amount, duration));
    }

    public void AddBoostFireRateStackLeft(float amount, float duration)
    {
        fireRateBoostStacksLeft.Add(new ShipBonusStack(amount, duration));
    }
    
    public void AddBoostFireRateStackRight(float amount, float duration)
    {
        fireRateBoostStacksRight.Add(new ShipBonusStack(amount, duration));
    }

    public void CrippleMovementMult(float amount)
    {
        crippledSpeedMult = amount;
        
        engineFlameEffect.SetActive(crippledSpeedMult >= 1);
        engineSmokeEffect.SetActive(crippledSpeedMult < 1);
    }

    public void DisableFiringLeft() { firingEnabledLeft = false; }
    public void DisableFiringRight() { firingEnabledRight = false; }
    public void EnableFiringLeft() { firingEnabledLeft = true; }
    public void EnableFiringRight() { firingEnabledRight = true; }

    public void AddWeaponSpread(float duration)
    {
        spreadStacks.Add(duration);
    }
    private bool spreadEnabled = true;
    public void DisableSpread() { spreadEnabled = false; }
    public void EnableSpread() { spreadEnabled = true; }

    public void AddEnergyBall(int count)
    {
        numEnergyBalls += count;
        UpdateEnergyBallUI();
    }
    public void FireEnergyBall(float size)
    {
        if (energyBallPrefab != null && numEnergyBalls > 0)
        {
            Instantiate(energyBallPrefab, projSpawnFront.position, Quaternion.identity);

            PushInDir(-Vector2.up, 1000f);

            numEnergyBalls--;
            UpdateEnergyBallUI();
        }
    }

    protected void UpdateEnergyBallUI()
    {
        ExteriorManager.exteriorManager.UpdateEnergyBallUI(numEnergyBalls);
    }

    [SerializeField] int shields = 0;
    public int Shields { get { return shields; } }
    public int MaxShields { get { return shieldsSprites.Length; } }

    private bool shieldsEnabled = true;
    public bool ShieldsEnabled { get { return shieldsEnabled; } }
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
            if (shields-1 < shieldsSprites.Length) shieldsRenderer.sprite = shieldsSprites[shields-1];
        }
    }

    public void DisableShields() { shieldsEnabled = false; UpdateShieldsSprite(); }
    public void EnableShields() { shieldsEnabled = true; UpdateShieldsSprite(); }

    public void PushInDir(Vector2 dir, float force)
    {
        _rigidbody2D.AddForce(dir * force);
    }
}
