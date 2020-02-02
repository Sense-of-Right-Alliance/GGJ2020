using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpaceDrift : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector2 newPos = transform.position;
        newPos.y -= speed * Time.deltaTime;

        transform.position = newPos;
    }
}
