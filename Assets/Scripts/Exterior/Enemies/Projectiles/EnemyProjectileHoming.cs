using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyProjectileHoming : EnemyProjectile
{
    [SerializeField] float targetingRadius = 5f;
    [SerializeField] float rotateSpeed = 1f;
    Transform target;   

    protected override void UpdatePosition()
    {
        Vector3 endRay = transform.position;
        endRay.x += targetingRadius;
        Debug.DrawLine(transform.position, endRay);

        if (target == null)
        {
            AcquireTarget();
        }
        else
        {
            UpdateTargetVector();
        }

        base.UpdatePosition();
    }

    private void AcquireTarget()
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
