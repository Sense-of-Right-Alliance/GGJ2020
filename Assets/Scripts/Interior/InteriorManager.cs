using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorManager : MonoBehaviour
{
    public static InteriorManager interiorManager;

    [SerializeField] GameObject resourcePrefab;
    [SerializeField] GameObject flamePrefab;
    [SerializeField] GameObject[] debrisPrefabs;

    [SerializeField] InteriorLayout interiorLayout;

    [SerializeField] GameObject steamVentPrefab;

    [SerializeField] GameObject hullBreachPrefab;

    [SerializeField] ExteriorShip exteriorShip;
    [SerializeField] InteriorPlayer interiorPlayer;

    [SerializeField] GameObject interiorCamera;
    [SerializeField] GameObject interiorCameraQuad;
    [SerializeField] GameObject interiorShipMap;

    [SerializeField] AudioClip[] hullHitSounds;
    [SerializeField] AudioClip[] shieldHitSounds;
    [SerializeField] AudioClip loadItemSfX;

    [SerializeField] ShipHPUI shipHPUI;

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

        occupiedDebrisLocations = new GameObject[interiorLayout.DebrisLocations.Length];
        occupiedSteamLocations = new GameObject[interiorLayout.SteamVentLocations.Length];
        occupiedHullBreachLocations = new GameObject[interiorLayout.HullBreachLocations.Length];

        if (interiorPlayer == null) interiorPlayer = GameObject.FindObjectOfType<InteriorPlayer>();
        if (interiorCamera == null) interiorCamera = GameObject.Find("InteriorCamera");
        if (interiorCameraQuad == null) interiorCameraQuad = GameObject.Find("InteriorCameraQuad");
        if (interiorShipMap == null) interiorShipMap = GameObject.Find("ShipInteriorMap");

        GameObject exteriorShipObj = GameObject.Find("ExteriorShip");
        if (exteriorShip == null && exteriorShipObj != null) exteriorShip = exteriorShipObj.GetComponent<ExteriorShip>();

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

        if (exteriorShip != null)
        {
            exteriorShip.exteriorShipMoveEvent.AddListener(AddInertiaVelocityToObjects);
            exteriorShip.shipHitEvent.AddListener(OnExternalShipDamaged);
            shipHPUI.InitPips(exteriorShip);
        }
        else
        {
            Debug.Log("InteriorManager: Unable to find game object with name 'ExternalShip' to add events listeners to");
        }

        UpdateSiren();
    }

    private void OnExternalShipDamaged(GameObject damager)
    {
        InteriorProblemMaker problemMaker = damager.GetComponent<InteriorProblemMaker>();
        if (problemMaker)
        {
            HandleShipDamage(problemMaker.ProblemOdds);
        }
    }

    private void Update()
    {
        UpdateSiren();
    }

    private void UpdateSiren()
    {
        if (numBreaches > 0 || numFlames > 0 || exteriorShip.HitPointPercent < 0.4f)
        {
            if (interiorLayout.Siren.Alert != Siren.AlertState.Red) interiorLayout.Siren.SetAlert(Siren.AlertState.Red);
        }
        else if (numDebris > 0 || numVents > 0 || exteriorShip.HitPointPercent < 0.8f)
        {
            if (interiorLayout.Siren.Alert != Siren.AlertState.Yellow) interiorLayout.Siren.SetAlert(Siren.AlertState.Yellow);
        }
        else if (interiorLayout.Siren.Alert != Siren.AlertState.None) interiorLayout.Siren.SetAlert(Siren.AlertState.None);
    }

    // Spawns a resource game object inside the ship, which the interior player can pickup and drop off at a station
    public void SpawnResource()
    {
        GameObject resource = GameObject.Instantiate<GameObject>(resourcePrefab, interiorLayout.ResourceSpawnLocation.position, Quaternion.identity);
        resource.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        resource.transform.Rotate(Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)).eulerAngles);

        if (transform.parent != null) resource.transform.SetParent(transform.parent);
        spawnedResources.Add(resource);

        ScoreManager.scoreManager.ExteriorResourcePickedUp();

        aSource.PlayOneShot(loadItemSfX, 0.5f);
    }

    public void ReclaimJettisonedObject(GameObject reclaimedObject)
    {
        Transform t = reclaimedObject.transform;
        
        t.position = interiorLayout.ResourceSpawnLocation.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        if (reclaimedObject.tag != "Player") t.Rotate(Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)).eulerAngles);

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

    public void HandleShipDamage(InteriorProblemOdds problemOdds = null)
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

        if (problemOdds == null)
        {
            problemOdds = new InteriorProblemOdds();
            problemOdds.nothingOdds = 100f;
            problemOdds.debrisOdds = 100f;
            problemOdds.steamOdds = 100f;
            problemOdds.flameOdds = 100f;
            problemOdds.breachOdds = 100f;
        }

        int problemsSpawned = 0;

        // Problems that can occur when shields are up
        int steamSpawned = CheckSpawnSteamVents(problemOdds.ComputeChanceForProblem(InteriorProblemType.Steam), problemOdds.numProblems);

        problemsSpawned += steamSpawned;

        // Problems that can only occur when shields are down
        if (exteriorShip.Shields <= 0 || !exteriorShip.ShieldsEnabled)
        {
            aSource.PlayOneShot(hullHitSounds[Random.Range(0, hullHitSounds.Length)], 0.5f);

            int fSpawned = CheckSpawnFlame(problemOdds.ComputeChanceForProblem(InteriorProblemType.Flame), problemOdds.numProblems);
            int dSpawned = CheckSpawnDebris(problemOdds.ComputeChanceForProblem(InteriorProblemType.Debris), problemOdds.numProblems);
            int hSpawned = CheckSpawnHullBreach(problemOdds.ComputeChanceForProblem(InteriorProblemType.Breach), problemOdds.numProblems);

            problemsSpawned += fSpawned + dSpawned + hSpawned;
        }
        else
        {
            aSource.PlayOneShot(shieldHitSounds[Random.Range(0, shieldHitSounds.Length)], 0.5f);
        }

        if (problemsSpawned == 0) SpawnSteamVent(1); // always spawn at least one problem TEMPORARY FIX

        UpdateShipHPUI();
    }

    // Flames only happen on stations and prevent them from functioning
    private int CheckSpawnFlame(float odds, int num)
    {
        int count = 0;
        for (int i = 0; i < num; i++)
        {
            if (Random.value <= odds) count++;
        }

        if (count > 0) SpawnStationFlames(count);

        return count;
    }

    private void SpawnStationFlames(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (interiorLayout.Stations.Length > 0)
            {
                Station igniteStation = interiorLayout.Stations[Random.Range(0, interiorLayout.Stations.Length)];
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
    }

    private int CheckSpawnHullBreach(float odds, int num)
    {
        int count = 0;
        for (int i = 0; i < num; i++)
        {
            if (Random.value <= odds) count++;
        }

        if (count > 0) SpawnHullBreach(count);

        return count;
    }

    private void SpawnHullBreach(int amount)
    {
        List<int> availableBreachLocations = new List<int>();
        for (int i = 0; i < interiorLayout.HullBreachLocations.Length; i++)
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
            
            Transform t = interiorLayout.HullBreachLocations[index];
            GameObject hullBreach = Instantiate(hullBreachPrefab, t.position, t.rotation);
            if (transform.parent != null) hullBreach.transform.SetParent(transform.parent);

            hullBreach.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnHullBreachDestroyed);
            occupiedHullBreachLocations[index] = hullBreach;
            numBreaches++;
        }
    }
    
    private int CheckSpawnSteamVents(float odds, int num)
    {
        int count = 0;
        for (int i = 0; i < num; i++)
        {
            if (Random.value <= odds) count++;
        }

        if (count > 0) SpawnSteamVent(count);

        return count;
    }

    private void SpawnSteamVent(int amount)
    {
        List<int> availableSteamLocations = new List<int>();
        for (int i = 0; i < interiorLayout.SteamVentLocations.Length; i++)
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
            
            Transform t = interiorLayout.SteamVentLocations[index];
            GameObject steamVent = Instantiate(steamVentPrefab, t.position, t.rotation);
            if (transform.parent != null) steamVent.transform.SetParent(transform.parent);

            steamVent.GetComponent<InteriorProblem>().ProblemDestroyedOrRemovedEvent.AddListener(OnSteamVentDestroyed);
            occupiedSteamLocations[index] = steamVent;
            numVents++;
        }
    }

    private int CheckSpawnDebris(float odds, int num)
    {
        int count = 0;
        for (int i = 0; i < num; i++)
        {
            if (Random.value <= odds) count++;
        }

        if (count > 0) SpawnDebris(count);
        return count;
    }

    private void SpawnDebris(int amount)
    {
        List<int> availableDebrisLocations = new List<int>();
        for (int i = 0; i < interiorLayout.DebrisLocations.Length; i++)
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
            
            Transform t = interiorLayout.DebrisLocations[index];
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
        OnInteriorProblemFixed();
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
        OnInteriorProblemFixed();
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
        OnInteriorProblemFixed();
    }

    private void OnFlameDestroyed(GameObject gameObject)
    {
        numFlames--;

        OnInteriorProblemFixed();
    }

    private void OnInteriorProblemFixed()
    {
        ScoreManager.scoreManager.InteriorProblemFixed();
        exteriorShip.RepairDamage(1);
        UpdateShipHPUI();

    }

    private void UpdateShipHPUI()
    {
        shipHPUI.UpdatePips();
    }

    // Push all interior objects in this direction, to simulate inertia of the ship moving around
    private void AddInertiaVelocityToObjects(ExteriorShip ship)
    {
        Vector2 velocity = ship.lastVelocity;

        Vector2 dir = velocity.normalized;
        float mag = velocity.magnitude * 0.25f;

        interiorPlayer.PushInDir(dir, mag);

        // Push anything that can be pushed! Really shake things up.
        Pushable[] pushables = GameObject.FindObjectsOfType<Pushable>();
        for (int i = 0; i < pushables.Length; i++)
        {
            pushables[i].PushInDir(dir, mag);
        }
    }
}
