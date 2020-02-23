using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JettisonedObject : MonoBehaviour
{
    private GameObject interiorObject;
    public GameObject InteriorObject { get { return interiorObject; } }

    public void SetInteriorObject(GameObject interiorObject)
    {
        this.interiorObject = interiorObject;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer interiorSR = interiorObject.GetComponent<SpriteRenderer>();
        if (interiorSR == null) interiorSR = interiorObject.GetComponentInChildren<SpriteRenderer>();

        sr.sprite = interiorSR.sprite;
    }
}
