using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnTransform;
    [SerializeField] private float shootDelay = 0.2f;
    [SerializeField] int spreadShot = 0;
    [SerializeField] float targetingRadius = 10f;
    [SerializeField] float rotateSpeed = 1f;

    Transform target;

    protected Vector2 _direction = -Vector2.up;

    private AudioSource _audioSource;

    private float _shootTimer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        Asteroid a = transform.GetComponentInParent<Asteroid>();
        if (a != null) a.BlownUpEvent.AddListener(BlowUp);
    }

    private void BlowUp(GameObject obj)
    {
        ScoreManager.scoreManager.EnemyDestroyed(10);
    }

    private void Update()
    {
        if (target == null)
        {
            Vector3 endRay = transform.position;
            endRay.x += targetingRadius;
            Debug.DrawLine(transform.position, endRay);

            AcquireTarget();
        }
        else
        {
            UpdateTargetVector();
            UpdateShoot();
        }
        
    }

    private void AcquireTarget()
    {
        _shootTimer -= Time.deltaTime;

        if (_shootTimer <= 0f) // don't update every frame in case this physics calc is expensive
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetingRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {
                    //Debug.Log("Target Acquired!");
                    target = hitColliders[i].transform;
                    break;
                }
                i++;
            }

            _shootTimer = shootDelay;
        }
    }

    private void UpdateShoot()
    {
        if (projectilePrefab != null)
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
        GameObject proj = Instantiate(projectilePrefab, projectileSpawnTransform.position, transform.rotation);

        proj.GetComponent<EnemyProjectile>().SetDirection(_direction);


        if (spreadShot > 0)
        {
            float fanAngle = 45f;
            for (int i = 0; i < spreadShot; i++)
            {
                Quaternion rotation = transform.rotation;
                GameObject fanProj = Instantiate(projectilePrefab, projectileSpawnTransform.position, rotation);
                fanProj.GetComponent<EnemyProjectile>().SetDirection(_direction);
                fanProj.transform.Rotate(new Vector3(0, 0, -fanAngle + (fanAngle / spreadShot) * i));

                fanProj = Instantiate(projectilePrefab, projectileSpawnTransform.position, rotation);
                fanProj.transform.Rotate(new Vector3(0, 0, fanAngle - (fanAngle / spreadShot) * i));
            }
        }
    }

    private void UpdateTargetVector()
    {
        // Determine which direction to rotate towards
        Vector2 targetDirection = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector2 newDirection = Vector3.RotateTowards(_direction, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        var angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        //transform.rotation = Quaternion.LookRotation(newDirection);

        _direction = newDirection;
    }
}
