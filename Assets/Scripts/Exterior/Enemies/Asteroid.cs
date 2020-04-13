using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    public UnityGameObjectEvent EnemyDestroyedOrRemovedEvent;
    public UnityGameObjectEvent BlownUpEvent;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionSFXOverride;
    [SerializeField] private float explosionVolume = 0.5f;
    [SerializeField] private List<GameObject> childAsteroidPrefabs;
    [SerializeField] private int hitPoints = 12;

    [SerializeField] bool breakAgainstOtherAsteroids = false;

    [SerializeField] int scoreValue = 100;
    public int ScoreValue { get { return scoreValue; } }

    [SerializeField] float shrapnelForce = 0;
    
    private Quaternion _rotationAmount;
    private MovementBehaviour _movementBehaviour;

    private void Awake()
    {
        if (EnemyDestroyedOrRemovedEvent == null) EnemyDestroyedOrRemovedEvent = new UnityGameObjectEvent();
        if (BlownUpEvent == null) BlownUpEvent = new UnityGameObjectEvent();
    }

    private void Start()
    {
        //_movementBehaviour = GetComponent<MovementBehaviour>();
        //_movementBehaviour.direction = -transform.up;

        _rotationAmount = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-2.5f / hitPoints, 2.5f / hitPoints));
    }

    private void Update()
    {
        transform.Rotate(_rotationAmount.eulerAngles);
    }

    public void TakeHit(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            BlowUp();
        }
        else
        {
            CheckForResourceDrop(false);
        }
    }

    private void BlowUp()
    {
        CheckForResourceDrop(true);

        if (explosionPrefab != null)
        {
            GameObject obj = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (explosionSFXOverride != null)
            {
                obj.GetComponent<AudioSource>().clip = explosionSFXOverride;
                obj.GetComponent<AudioSource>().volume = explosionVolume;
                obj.GetComponent<AudioSource>().Play();
            }
        }

        //List<GameObject> spawnedAsteroids = new List<GameObject>();
        foreach (var childAsteroidPrefab in childAsteroidPrefabs)
        {
            Vector2 spawnPos = transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0f);
            GameObject gObj = Instantiate(childAsteroidPrefab, spawnPos, Quaternion.Euler(0, 0, Random.Range(-45, 45)));
            if (shrapnelForce > 0f)
            {
                Vector3 dir = (gObj.transform.position - transform.position).normalized;
                gObj.GetComponent<Rigidbody2D>().AddForce(dir * shrapnelForce);

                //gObj.GetComponent<MovementBehaviour>().direction = dir;
            }
        }

        BlownUpEvent.Invoke(gameObject);

        //ScoreManager.scoreManager.AsteroidDestroyed(scoreValue);
        Remove();
    }

    public void Remove()
    {
        EnemyDestroyedOrRemovedEvent.Invoke(gameObject);
        EnemyDestroyedOrRemovedEvent.RemoveAllListeners();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ExteriorShip>().TakeHit(gameObject);//1, problemOdds); // deals only 1 damage because we're not masochists
            TakeHit(breakAgainstOtherAsteroids ? this.hitPoints : 1);
        }
        else if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("AmbushEnemy"))
        {
            collision.gameObject.GetComponent<Enemy>().BlowUp();
            if (breakAgainstOtherAsteroids) TakeHit(this.hitPoints);
        }
        else if (breakAgainstOtherAsteroids && collision.transform.CompareTag("Asteroid"))
        {
            TakeHit(this.hitPoints);
        }
    }

    private void CheckForResourceDrop(bool destroy)
    {
        ExteriorResourceDrop drop = GetComponent<ExteriorResourceDrop>();
        if (drop)
        {
            if (destroy) drop.HandleDestroy();
            else drop.HandleHit();
        }
    }
}
