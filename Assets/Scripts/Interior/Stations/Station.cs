using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Station : MonoBehaviour
{
    [SerializeField] Ship exteriorShip;

    protected Ship Ship { get { return exteriorShip; }  }

    private void Awake()
    {
        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<Ship>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            ProcessResource(collision.gameObject.GetComponent<Resource>());
            Destroy(collision.gameObject);
        }
    }

    protected virtual void ProcessResource(Resource r)
    {
        
    }
}
