using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorManager : MonoBehaviour
{
    public static InteriorManager interiorManager;

    [SerializeField] GameObject resourcePrefab;
    [SerializeField] Transform resourceSpawn;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] GameObject[] debrisPrefabs;
    [SerializeField] Transform[] debrisLocations;

    [SerializeField] GameObject steamVentPrefab;
    [SerializeField] Transform[] steamVentLocations;

    [SerializeField] float flameChance = 0.6f;

    [SerializeField] Ship exteriorShip;
    [SerializeField] InteriorPlayer interiorPlayer;
    [SerializeField] Station[] stations;

    [SerializeField] GameObject interiorCamera;
    [SerializeField] GameObject interiorCameraQuad;
    [SerializeField] GameObject interiorShipMap;

    List<GameObject> spawnedResources = new List<GameObject>();

    private List<int> occupiedDebrisLocations = new List<int>();
    private List<int> occupiedSteamLocations = new List<int>();

    private void Awake()
    {
        InteriorManager.interiorManager = this;

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
        GameObject resource = GameObject.Instantiate<GameObject>(resourcePrefab, resourceSpawn.position, Quaternion.identity);

        spawnedResources.Add(resource);
    }

    public void ConsumeResource(GameObject r)
    {
        if (r.transform.parent != null)
        {
            spawnedResources.Remove(r.transform.parent.gameObject);
            Destroy(r.transform.parent.gameObject);
        }
        else
        {
            spawnedResources.Remove(r);
            Destroy(r);
        }
    }

    public void HandleShipDamage()
    {
        interiorCameraQuad.GetComponent<CameraShake>().Shake(0.3f,0.005f);
        interiorShipMap.GetComponent<CameraShake>().Shake(0.3f, 0.005f);

        interiorPlayer.DropItem();
        interiorPlayer.RandomPush();

        CheckSpawnSteamVents();
        CheckSpawnDebris();

        // Push anything that can be pushed! Really shake things up.
        Pushable[] pushables = GameObject.FindObjectsOfType<Pushable>();
        for (int i = 0; i < pushables.Length; i++)
        {
            pushables[i].RandomPush();
        }

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

    private void CheckSpawnSteamVents()
    {
        int numVents = 0;
        float r = Random.value;
        if (r > 0.5f) numVents = 0;
        else if (r > 0.2f) numVents = 1;
        else if (r > 0.1f) numVents = 2;
        else numVents = 3;

        numVents = 1; // DEBUG

        SpawnSteamVent(numVents);
    }

    private void SpawnSteamVent(int amount)
    {
        List<int> availableSteamLocations = new List<int>();
        for (int i = 0; i < steamVentLocations.Length; i++)
        {
            if (!occupiedSteamLocations.Contains(i))
            {
                availableSteamLocations.Add(i);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            if (occupiedSteamLocations.Count == steamVentLocations.Length)
            {
                Debug.Log("InteriorManager: All steam vent locaitons occupied!");
                break;
            }

            int index = availableSteamLocations[Random.Range(0, availableSteamLocations.Count)];
            occupiedSteamLocations.Add(index);
            Transform t = steamVentLocations[index];
            GameObject debris = Instantiate(steamVentPrefab, t.position, t.rotation);
        }
    }

    private void CheckSpawnDebris()
    {
        int numDebris = 0;
        float r = Random.value;
        if (r > 0.5f) numDebris = 0;
        else if (r > 0.2f) numDebris = 1;
        else if (r > 0.1f) numDebris = 2;
        else numDebris = 3;

        SpawnDebris(numDebris);
    }

    private void SpawnDebris(int amount)
    {
        List<int> availableDebrisLocations = new List<int>();
        for (int i = 0; i < debrisLocations.Length; i++)
        {
            if (!occupiedDebrisLocations.Contains(i))
            {
                availableDebrisLocations.Add(i);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            if (occupiedDebrisLocations.Count == debrisLocations.Length)
            {
                Debug.Log("InteriorManager: All debris locaitons occupied!");
                break;
            }

            int index = availableDebrisLocations[Random.Range(0, availableDebrisLocations.Count)];
            occupiedDebrisLocations.Add(index);
            Transform t = debrisLocations[index];
            GameObject debris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], t.position, t.rotation);
        }
    }
}
