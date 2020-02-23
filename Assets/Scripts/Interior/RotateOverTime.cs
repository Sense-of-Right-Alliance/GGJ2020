using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] float speed = 100f;


    private void Start()
    {

    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * (speed * Time.deltaTime));
    }
}
