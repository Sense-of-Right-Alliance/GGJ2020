using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Object will pace back and forth along the top of the screen
public class MovementBehaviourStay : MovementBehaviour
{
    protected Rect bounds;

    protected override void StartProjectile()
    {
        bounds = ExteriorManager.exteriorManager.Bounds;
    }

    protected override void UpdateMovement()
    {
        if (!isInPosition)
        {
            _rigidbody2D.AddForce(speed * direction.normalized);

            Debug.DrawLine(new Vector2(bounds.xMin, bounds.yMax - (bounds.height * 0.05f)), new Vector2(bounds.xMax, bounds.yMax - (bounds.height * 0.05f)));

            if (transform.position.y <= bounds.yMax - (bounds.height * 0.05f))
            {
                isInPosition = true;
            }
        }
    }
}
