using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BackgroundQuadScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;
    Material myMaterial;
    Vector2 offset;

    private void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0, scrollSpeed);
    }

    private void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
