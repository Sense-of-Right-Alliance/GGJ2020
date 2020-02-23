using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public enum PlayerID { Player1, Player2 }

[Serializable]
public enum ShipRole { Engineer, Pilot }

public class LaunchPad : MonoBehaviour
{
    [SerializeField] ShipRole role = ShipRole.Engineer;
    public ShipRole Role { get { return role; } }

    [SerializeField] int sceneID = 2;
    public int SceneID { get { return sceneID; } }

    private PlayerID? readyID = null;
    public PlayerID? ReadyID { get { return readyID; } }

    private float t = 0f;
    private Color startColor = Color.grey;
    private Color endColor = Color.black;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Ready(PlayerID id)
    {
        Debug.Log("LaunchPad " + role.ToString() + " -> Player " + id.ToString());
        readyID = id;

        SpaceStationManager.stationManager.PadReady(this);

        _spriteRenderer.color = Color.white;
    }

    public void Unready()
    {
        Debug.Log("LaunchPad " + role.ToString() + " Unready");
        readyID = null;

        SpaceStationManager.stationManager.PadUnready(this);

        _spriteRenderer.color = Color.grey;
    }

    private void Update()
    {
        if (readyID == null)
        {
            t += 0.75f * Time.deltaTime;
            Color c = Color.Lerp(startColor, endColor, Mathf.Pow(t,2f));

            _spriteRenderer.color = c;

            if (t >= 1f)
            {
                t = 0f;
                Color temp = startColor;
                startColor = endColor;
                endColor = temp;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interior Player")
        {

        }
    }
}
