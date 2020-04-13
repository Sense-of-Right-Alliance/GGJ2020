using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{

}

public class Enemy : MonoBehaviour
{
    public UnityGameObjectEvent EnemyDestroyedOrRemovedEvent;
    public UnityGameObjectEvent BlownUpEvent;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int hitPoints = 2;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnTransform;
    [SerializeField] private float shootDelay = 0.2f;
    [SerializeField] Quaternion shootDirection = Quaternion.identity;

    [SerializeField] int scoreValue = 100;
    public int ScoreValue { get { return scoreValue; } }

    [SerializeField] InteriorProblemOdds problemOdds;

    [SerializeField] int spreadShot = 0;
    [SerializeField] float spreadRadius = 45f;

    private Rect _bounds;

    private MovementBehaviour _movementBehaviour;
    private AudioSource _audioSource;

    private float _shootTimer;
    

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _movementBehaviour = GetComponent<MovementBehaviour>();

        EnemyDestroyedOrRemovedEvent = new UnityGameObjectEvent();
        BlownUpEvent = new UnityGameObjectEvent();
    }

    private void Start()
    {
        _movementBehaviour.direction = -transform.up;

        _shootTimer = shootDelay;

        _bounds = ExteriorManager.exteriorManager.Bounds;
    }

    private bool CheckOnScreen()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        return x > _bounds.xMin && x < _bounds.xMax && y > _bounds.yMin && y < _bounds.yMax;
    }

    private void Update()
    {
        if (projectilePrefab != null && CheckOnScreen() && shootDelay >= 0)
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer <= 0f)
            {
                Shoot();
                _shootTimer = shootDelay;
            }
        }
    }

    private void Shoot()
    {
        _audioSource.Play();
        Instantiate(projectilePrefab, projectileSpawnTransform.position, transform.rotation * shootDirection);

        if (spreadShot > 0)
        {
            for (int i = 0; i < spreadShot; i++)
            {
                Quaternion rotation = transform.rotation * shootDirection;
                GameObject obj = Instantiate(projectilePrefab, projectileSpawnTransform.position, rotation);
                obj.transform.Rotate(new Vector3(0, 0, -spreadRadius + (spreadRadius / spreadShot) * i));

                obj = Instantiate(projectilePrefab, projectileSpawnTransform.position, rotation);
                obj.transform.Rotate(new Vector3(0, 0, spreadRadius - (spreadRadius / spreadShot) * i));
            }
        }
    }

    public void TakeHit(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            BlowUp();
        } else
        {
            CheckForResourceDrop(false);
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

    public void BlowUp(bool canDropResource = true)
    {
        //ScoreManager.scoreManager.EnemyDestroyed(scoreValue);
        BlownUpEvent.Invoke(gameObject);

        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        CheckForResourceDrop(true);

        Remove();
    }

    public void Remove()
    {
        EnemyDestroyedOrRemovedEvent.Invoke(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ExteriorShip>().TakeHit(1, problemOdds); // deals only 1 damage because we're not masochists
            Destroy(gameObject);
        }
    }
}
