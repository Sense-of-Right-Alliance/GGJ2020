using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourcePip : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFull(bool full)
    {
        spriteRenderer.color = full ? Color.white : Color.grey;
    }
}
