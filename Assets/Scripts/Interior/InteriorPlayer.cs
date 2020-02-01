using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorPlayer : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Vector2 velocity = Vector2.zero;

    Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        velocity *= 0.5f;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x += speed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            velocity.y += speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.y -= speed;
        }

        rigidbody2D.AddForce(velocity);
    }
}
