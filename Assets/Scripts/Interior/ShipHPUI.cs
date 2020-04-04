using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipHPUI : MonoBehaviour
{
    [SerializeField] GameObject pipPrefab;

    protected ResourcePip[] pips;
    protected ExteriorShip ship;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public virtual void InitPips(ExteriorShip ship)
    {
        this.ship = ship;

        this.pips = new ResourcePip[ship.MaxHitPoints];

        PositionPips(ship.MaxHitPoints);
        
        UpdatePips();
    }

    protected void PositionPips(int numPips)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < numPips; i++)
        {
            GameObject pip = GameObject.Instantiate<GameObject>(pipPrefab, Vector2.zero, Quaternion.identity);

            // y
            Vector3 pipPos = transform.position;
            //pipPos.y -= (spriteRenderer.sprite.bounds.size.y / 2f) + (spriteRenderer.sprite.bounds.size.y * 0.2f);
            // x
            float pipWidth = pip.GetComponent<SpriteRenderer>().sprite.bounds.size.x * pip.transform.localScale.x;
            float pipBuffer = pipWidth * 0.15f;
            float pipBarLength = (pipWidth * (float)numPips) + (pipBuffer * (float)(numPips - 1));
            float halfLength = pipBarLength / 2.0f;
            pipPos.x = (pipWidth / 2.0f) + pipPos.x - halfLength + ((pipWidth + pipBuffer) * i);

            pip.transform.position = pipPos;
            pip.transform.SetParent(transform);

            pips[i] = pip.GetComponent<ResourcePip>();
        }
    }

    public void UpdatePips()
    {
        for (int i = 0; i < pips.Length; i++)
        {
            pips[i].SetFull(i < ship.HitPoints);
        }
    }
}
