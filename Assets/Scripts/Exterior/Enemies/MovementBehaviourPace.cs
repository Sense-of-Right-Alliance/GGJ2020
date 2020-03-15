using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Object will pace back and forth along the top of the screen
public class MovementBehaviourPace : MovementBehaviour
{
    public bool isPacing = false;

    protected Rect bounds;

    protected override void StartProjectile()
    {
        bounds = ExteriorManager.exteriorManager.Bounds;
    }

    protected override void UpdateMovement()
    {
        _rigidbody2D.AddForce(speed * direction.normalized);

        if (!isPacing && transform.position.y <= bounds.yMax - (bounds.height * 0.05f))
        {
            direction = Vector2.right; // start pacing
            isPacing = true;
        }
        else
        {
            Debug.DrawLine(new Vector2(bounds.xMin, bounds.yMax - (bounds.height * 0.05f)), new Vector2(bounds.xMax, bounds.yMax - (bounds.height * 0.05f)));
        }

        if (isPacing && ((direction.x > 0 && transform.position.x >= bounds.xMax - (bounds.width * 0.05f))
            || (direction.x < 0 && transform.position.x <= bounds.xMin + (bounds.width * 0.05f))))
        {
            direction.x *= -1; // change horizontal direction
        }
    }
}
