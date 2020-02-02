using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Station : MonoBehaviour
{
    [SerializeField] Ship exteriorShip;
    [SerializeField] bool activated = true;

    protected Ship Ship { get { return exteriorShip; }  }

    private void Awake()
    {
        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<Ship>();
    }

    private void Start()
    {
        activated = true;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated && collision.tag == "Interior Resource")
        {
            ProcessResource(collision.gameObject.GetComponent<Resource>());
            Destroy(collision.gameObject);
        }
    }

    protected virtual void ProcessResource(Resource r)
    {
        
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

    public virtual void Reactivate()
    {
        activated = true;
    }
}
