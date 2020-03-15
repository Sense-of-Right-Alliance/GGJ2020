using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorLayout : MonoBehaviour
{
    [SerializeField] Transform resourceSpawnLocation;
    [SerializeField] Transform[] debrisLocations;
    [SerializeField] Transform[] steamVentLocations;
    [SerializeField] Transform[] hullBreachLocations;
    [SerializeField] Station[] stations;
    [SerializeField] Siren siren;

    public Transform ResourceSpawnLocation { get { return resourceSpawnLocation; } }
    public Transform[] DebrisLocations { get { return debrisLocations; } }
    public Transform[] SteamVentLocations { get { return steamVentLocations; } }
    public Transform[] HullBreachLocations { get { return hullBreachLocations; } }
    public Station[] Stations { get { return stations; } }
    public Siren Siren { get { return siren; } }
}
