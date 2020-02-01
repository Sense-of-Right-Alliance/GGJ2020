using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorCamera : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (followTarget != null)
        {
            Vector2 targetPos = followTarget.position;
            Vector3 newPos = targetPos;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}
