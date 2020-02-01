using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorManager : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] Transform resourceSpawn;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Debug!
        {
            SpawnResource();
        }
    }

    // Spawns a resource game object inside the ship, which the interior player can pickup and drop off at a station
    public void SpawnResource()
    {
        GameObject.Instantiate<GameObject>(resourcePrefab, resourceSpawn.position, Quaternion.identity);
    }
}
