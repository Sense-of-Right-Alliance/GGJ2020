using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] private Vector2 direction = Vector2.down;

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

    private void UpdateMovement()
    {
        rigidbody2D.AddForce(speed * direction);
    }
}
