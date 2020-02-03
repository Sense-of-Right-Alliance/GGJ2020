using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestoryAfterTime : MonoBehaviour
{
    [SerializeField] float duration = 2f;

    private void Start()
    {
        
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
