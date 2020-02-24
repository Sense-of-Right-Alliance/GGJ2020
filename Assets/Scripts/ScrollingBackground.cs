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
        spriteHeight = spriteRenderer.sprite.bounds.size.y * backgrounds[0].transform.localScale.y;

        Vector2 newPos = backgrounds[0].localPosition;
        newPos.y = backgrounds[1].localPosition.y + spriteHeight;

        backgrounds[0].localPosition = newPos;
    }

    private void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector2 newPos = backgrounds[i].localPosition;
            newPos.y -= scrollSpeed * Time.deltaTime;
            
            if (newPos.y < -spriteHeight * 1f)
            {
                newPos.y = backgrounds[(i+1) % backgrounds.Length].localPosition.y + spriteHeight;
            }
            backgrounds[i].localPosition = newPos;
        }
    }
}
