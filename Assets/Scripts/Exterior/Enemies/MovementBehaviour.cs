using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    public Vector2 direction = Vector2.down;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        _rigidbody2D.AddForce(speed * direction.normalized);
    }
}
