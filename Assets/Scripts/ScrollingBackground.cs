using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float scrollSpeed = 5f;
    
    float spriteHeight = 1f;
    
    private void Start()
    {
        SpriteRenderer spriteRenderer = backgrounds[0].GetComponent<SpriteRenderer>();
        spriteHeight = spriteRenderer.sprite.bounds.size.y;

        Vector2 newPos = backgrounds[0].position;
        newPos.y = backgrounds[1].position.y + spriteHeight;

        backgrounds[0].position = newPos;
    }

    private void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector2 newPos = backgrounds[i].position;
            newPos.y -= scrollSpeed * Time.deltaTime;
            
            if (newPos.y < -spriteHeight)
            {
                newPos.y = backgrounds[(i+1) % backgrounds.Length].position.y + spriteHeight;
            }
            backgrounds[i].position = newPos;
        }
    }
}
