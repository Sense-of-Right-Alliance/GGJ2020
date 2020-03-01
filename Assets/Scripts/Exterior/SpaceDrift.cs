using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpaceDrift : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector2 direction = Vector2.down;
    [SerializeField] Vector2 diftDirection = Vector2.zero;

    private void Start()
    {
        
    }

    public void SetDrifDirection(Vector2 newDirection)
    {
        diftDirection = newDirection;
    }

    private void Update()
    {
        Vector2 newPos = transform.position;
        newPos += (direction + diftDirection) * speed * Time.deltaTime;

        transform.position = newPos;
    }
}
