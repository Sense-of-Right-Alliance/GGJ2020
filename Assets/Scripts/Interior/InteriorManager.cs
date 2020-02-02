using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorManager : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] Transform resourceSpawn;
    [SerializeField] GameObject flamePrefab;

    [SerializeField] float flameChance = 0.6f;

    [SerializeField] Ship exteriorShip;
    [SerializeField] InteriorPlayer interiorPlayer;
    [SerializeField] Station[] stations;

    [SerializeField] GameObject interiorCamera;
    [SerializeField] GameObject interiorCameraQuad;
    [SerializeField] GameObject interiorShipMap;

    private void Awake()
    {
        if (interiorPlayer == null) interiorPlayer = GameObject.FindObjectOfType<InteriorPlayer>();
        if (interiorCamera == null) interiorCamera = GameObject.Find("InteriorCamera");
        if (interiorCameraQuad == null) interiorCameraQuad = GameObject.Find("InteriorCameraQuad");
        if (interiorShipMap == null) interiorShipMap = GameObject.Find("ShipInteriorMap");

        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<Ship>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R)) // Debug!
        {
            SpawnResource();
        }
        */
    }

    // Spawns a resource game object inside the ship, which the interior player can pickup and drop off at a station
    public void SpawnResource()
    {
        GameObject.Instantiate<GameObject>(resourcePrefab, resourceSpawn.position, Quaternion.identity);
    }

    public void HandleShipDamage()
    {
        interiorCameraQuad.GetComponent<CameraShake>().Shake(0.3f,0.005f);
        interiorShipMap.GetComponent<CameraShake>().Shake(0.3f, 0.005f);

        interiorPlayer.DropResource();
        interiorPlayer.RandomPush();

        if (exteriorShip.Shields <= 0 && Random.value < flameChance && stations.Length > 0)
        {
            Station igniteStation = stations[Random.Range(0, stations.Length)];
            if (flamePrefab != null)
            {
                GameObject flame = GameObject.Instantiate<GameObject>(flamePrefab, igniteStation.gameObject.transform.position, Quaternion.identity);
                flame.GetComponent<Flame>().Ignite(igniteStation);
            }
        }
    }
}
