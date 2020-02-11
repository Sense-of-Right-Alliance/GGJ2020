using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

    public void Ready(PlayerID id)
    {
        Debug.Log("LaunchPad " + role.ToString() + " -> Player " + id.ToString());
        readyID = id;

        StationManager.stationManager.PadReady(this);
    }

    public void Unready()
    {
        Debug.Log("LaunchPad " + role.ToString() + " Unready");
        readyID = null;

        StationManager.stationManager.PadUnready(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interior Player")
        {

        }
    }
}
