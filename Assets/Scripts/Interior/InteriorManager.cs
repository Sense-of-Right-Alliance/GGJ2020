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

    [SerializeField] GameObject hullBreachPrefab;
    [SerializeField] Transform[] hullBreachLocations;

    [SerializeField] float flameChance = 0.6f;

    [SerializeField] Ship exteriorShip;
    [SerializeField] InteriorPlayer interiorPlayer;
    [SerializeField] Station[] stations;

    [SerializeField] GameObject interiorCamera;
    [SerializeField] GameObject interiorCameraQuad;
    [SerializeField] GameObject interiorShipMap;

    [SerializeField] Siren siren;

    [SerializeField] AudioClip[] hullHitSounds;
    [SerializeField] AudioClip[] shieldHitSounds;
    [SerializeField] AudioClip loadItemSfX;

    List<GameObject> spawnedResources = new List<GameObject>();

    private GameObject[] occupiedDebrisLocations;
    private GameObject[] occupiedSteamLocations;
    private GameObject[] occupiedHullBreachLocations;

    private int numDebris = 0;
    private int numVents = 0;
    private int numBreaches = 0;
    private int numFlames = 0;

    private AudioSource aSource;

    private void Awake()
    {
        InteriorManager.interiorManager = this;

        occupiedDebrisLocations = new GameObject[debrisLocations.Length];
        occupiedSteamLocations = new GameObject[steamVentLocations.Length];
        occupiedHullBreachLocations = new GameObject[hullBreachLocations.Length];

        if (interiorPlayer == null) interiorPlayer = GameObject.FindObjectOfType<InteriorPlayer>();
        if (interiorCamera == null) interiorCamera = GameObject.Find("InteriorCamera");
        if (interiorCameraQuad == null) interiorCameraQuad = GameObject.Find("InteriorCameraQuad");
        if (interiorShipMap == null) interiorShipMap = GameObject.Find("ShipInteriorMap");

        if (exteriorShip == null) exteriorShip = GameObject.Find("ExteriorShip").GetComponent<Ship>();

        aSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameObject[] debris = GameObject.FindGameObjectsWithTag("Debris");
        for (int i = 0; i < debris.Length; i++)
        {
            debris[i].GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnDebrisDestroyed);
        }
        numDebris = debris.Length;

        GameObject[] vents = GameObject.FindGameObjectsWithTag("SteamVent");
        for (int i = 0; i < vents.Length; i++)
        {
            vents[i].GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnSteamVentDestroyed);
        }
        numVents = vents.Length;

        GameObject[] breaches = GameObject.FindGameObjectsWithTag("Breach");
        for (int i = 0; i < breaches.Length; i++)
        {
            breaches[i].GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnHullBreachDestroyed);
        }
        numBreaches = breaches.Length;

        GameObject[] flames = GameObject.FindGameObjectsWithTag("Flame");
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnFlameDestroyed);
        }
        numFlames = flames.Length;

        UpdateSiren();
    }

    private void Update()
    {
        UpdateSiren();
    }

    private void UpdateSiren()
    {
        if (numBreaches > 0 || numFlames > 0)
        {
            if (siren.Alert != Siren.AlertState.Red) siren.SetAlert(Siren.AlertState.Red);
        }
        else if (numDebris > 0 || numVents > 0)
        {
            if (siren.Alert != Siren.AlertState.Yellow) siren.SetAlert(Siren.AlertState.Yellow);
        }
        else if (siren.Alert != Siren.AlertState.None) siren.SetAlert(Siren.AlertState.None);
    }

    // Spawns a resource game object inside the ship, which the interior player can pickup and drop off at a station
    public void SpawnResource()
    {
        GameObject resource = GameObject.Instantiate<GameObject>(resourcePrefab, resourceSpawn.position, Quaternion.identity);
        if (transform.parent != null) resource.transform.SetParent(transform.parent);
        spawnedResources.Add(resource);

        aSource.PlayOneShot(loadItemSfX, 0.5f);
    }

    public void ReclaimJettisonedObject(GameObject reclaimedObject)
    {
        Transform t = reclaimedObject.transform;

        t.position = resourceSpawn.position;
        reclaimedObject.SetActive(true);

        aSource.PlayOneShot(loadItemSfX, 0.5f);
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
        // Shake things up
        interiorCameraQuad.GetComponent<CameraShake>().Shake(0.3f,0.005f);
        interiorShipMap.GetComponent<CameraShake>().Shake(0.3f, 0.005f);

        // Rough up the player
        interiorPlayer.DropItem();
        interiorPlayer.RandomPush();

        // Push anything that can be pushed! Really shake things up.
        Pushable[] pushables = GameObject.FindObjectsOfType<Pushable>();
        for (int i = 0; i < pushables.Length; i++)
        {
            pushables[i].RandomPush();
        }

        // ---- Spawn Problems for Intrior Player ---- //

        // Problems that can occur when shields are up
        CheckSpawnSteamVents();

        // Problems that can only occur when shields are down
        if (exteriorShip.Shields <= 0 || !exteriorShip.ShieldsEnabled)
        {
            aSource.PlayOneShot(hullHitSounds[Random.Range(0, hullHitSounds.Length)], 0.5f);

            CheckSpawnFlame();
            CheckSpawnDebris();
            CheckSpawnHullBreach();
        }
        else
        {
            aSource.PlayOneShot(shieldHitSounds[Random.Range(0, shieldHitSounds.Length)], 0.5f);
        }
    }

    // Flames only happen on stations and prevent them from functioning
    private void CheckSpawnFlame()
    {
        if (Random.value < flameChance && stations.Length > 0)
        {
            Station igniteStation = stations[Random.Range(0, stations.Length)];
            if (flamePrefab != null)
            {
                GameObject flame = GameObject.Instantiate<GameObject>(flamePrefab, igniteStation.gameObject.transform.position, Quaternion.identity);
                if (transform.parent != null) flame.transform.SetParent(transform.parent);
                flame.GetComponent<Flame>().Ignite(igniteStation);

                flame.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnFlameDestroyed);
                numFlames++;
            }
        }
    }

    private void CheckSpawnHullBreach()
    {
        int numBreaches = 0;
        float r = Random.value;
        if (r > 0.2f) numBreaches = 0;
        else numBreaches = 1;

        SpawnHullBreach(numBreaches);
    }

    private void SpawnHullBreach(int amount)
    {
        List<int> availableBreachLocations = new List<int>();
        for (int i = 0; i < hullBreachLocations.Length; i++)
        {
            if (occupiedHullBreachLocations[i] == null)
            {
                availableBreachLocations.Add(i);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            if (availableBreachLocations.Count == 0)
            {
                Debug.Log("InteriorManager: All breach locaitons occupied!");
                break;
            }

            int index = availableBreachLocations[Random.Range(0, availableBreachLocations.Count)];
            
            Transform t = hullBreachLocations[index];
            GameObject hullBreach = Instantiate(hullBreachPrefab, t.position, t.rotation);
            if (transform.parent != null) hullBreach.transform.SetParent(transform.parent);

            hullBreach.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnHullBreachDestroyed);
            occupiedHullBreachLocations[index] = hullBreach;
            numBreaches++;
        }
    }

    private void CheckSpawnSteamVents()
    {
        int numVents = 0;
        float r = Random.value;
        if (r > 0.5f) numVents = 0;
        else if (r > 0.2f) numVents = 1;
        else numVents = 2;

        SpawnSteamVent(numVents);
    }

    private void SpawnSteamVent(int amount)
    {
        List<int> availableSteamLocations = new List<int>();
        for (int i = 0; i < steamVentLocations.Length; i++)
        {
            if (occupiedSteamLocations[i] == null)
            {
                availableSteamLocations.Add(i);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            if (availableSteamLocations.Count == 0)
            {
                Debug.Log("InteriorManager: All steam vent locaitons occupied!");
                break;
            }

            int index = availableSteamLocations[Random.Range(0, availableSteamLocations.Count)];
            
            Transform t = steamVentLocations[index];
            GameObject steamVent = Instantiate(steamVentPrefab, t.position, t.rotation);
            if (transform.parent != null) steamVent.transform.SetParent(transform.parent);

            steamVent.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnSteamVentDestroyed);
            occupiedSteamLocations[index] = steamVent;
            numVents++;
        }
    }

    private void CheckSpawnDebris()
    {
        int numDebris = 0;
        float r = Random.value;
        if (r > 0.5f) numDebris = 0;
        else if (r > 0.2f) numDebris = 1;
        else numDebris = 2;

        SpawnDebris(numDebris);
    }

    private void SpawnDebris(int amount)
    {
        List<int> availableDebrisLocations = new List<int>();
        for (int i = 0; i < debrisLocations.Length; i++)
        {
            if (occupiedDebrisLocations[i] == null)
            {
                availableDebrisLocations.Add(i);
            }
        }

        for (int i = 0; i < amount; i++)
        {
            if (availableDebrisLocations.Count == 0)
            {
                Debug.Log("InteriorManager: All debris locaitons occupied!");
                break;
            }

            int index = availableDebrisLocations[Random.Range(0, availableDebrisLocations.Count)];
            
            Transform t = debrisLocations[index];
            GameObject debris = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], t.position, t.rotation);
            if (transform.parent != null) debris.transform.SetParent(transform.parent);

            debris.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnDebrisDestroyed);
            occupiedDebrisLocations[index] = debris;

            numDebris++;
        }
    }

    private void OnDebrisDestroyed(GameObject gameObject)
    {
        numDebris--;

        for (int i = 0; i < occupiedDebrisLocations.Length; i++)
        {
            if (occupiedDebrisLocations[i] == gameObject)
            {
                occupiedDebrisLocations[i] = null;
                break;
            }
        }
    }

    private void OnSteamVentDestroyed(GameObject gameObject)
    {
        numVents--;
        for (int i = 0; i < occupiedSteamLocations.Length; i++)
        {
            if (occupiedSteamLocations[i] == gameObject)
            {
                occupiedSteamLocations[i] = null;
                break;
            }
        }
    }

    private void OnHullBreachDestroyed(GameObject gameObject)
    {
        numBreaches--;
        for (int i = 0; i < occupiedHullBreachLocations.Length; i++)
        {
            if (occupiedHullBreachLocations[i] == gameObject)
            {
                occupiedHullBreachLocations[i] = null;
                break;
            }
        }
    }

    private void OnFlameDestroyed(GameObject gameObject)
    {
        numFlames--;
    }
}
