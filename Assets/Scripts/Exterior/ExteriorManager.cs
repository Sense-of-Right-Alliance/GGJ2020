using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ExteriorManager : MonoBehaviour
{
    public static ExteriorManager exteriorManager; // Singletoooooooonnnnnnn!

    public SpawnManager GetSpawnManager() { return _spawnManager; }
    public WaveManager GetWaveManager() { return _waveManager; }

    private WaveManager _waveManager;
    private SpawnManager _spawnManager;

    [SerializeField] Rect _bounds;
    public Rect Bounds { get { return _bounds; } }

    [SerializeField] ExteriorHUD exteriorHUD;

    private void Awake()
    {
        ExteriorManager.exteriorManager = this;
    }

    private void Start()
    {
        _waveManager = GetComponent<WaveManager>();
        _waveManager.StartWaves();
        _waveManager.WavesCompletedEvent.AddListener(OnMissionWavesComplete);

        _spawnManager = GetComponent<SpawnManager>();
        _spawnManager.EnemyDestroyedOrRemovedEvent.AddListener(OnEnemyDestroyedOrRemoved);
    }

    private void Update()
    {
        Debug.DrawLine(new Vector2(Bounds.xMin, Bounds.yMin), new Vector2(Bounds.xMax, Bounds.yMin));
        Debug.DrawLine(new Vector2(Bounds.xMin, Bounds.yMax), new Vector2(Bounds.xMax, Bounds.yMax));
    }

    private void OnEnemyDestroyedOrRemoved(GameObject enemy)
    {
        CheckEndMission();
    }

    public void HandleShipDestroyed(ExteriorShip ship)
    {
        _waveManager.StopWaves();

        GameObject.Find("ShipInteriorWalls").SetActive(false);
        GameObject.Find("ShipInteriorMap").SetActive(false);
        GameObject.Find("Stations").SetActive(false);
        GameObject.Find("Siren").SetActive(false);

        GameManager.gameManager.FailMission();
    }
    
    public void OnMissionWavesComplete()
    {
        CheckEndMission();
    }

    public void CheckEndMission()
    {
        if (_spawnManager.NumEnemies <= 0 && _waveManager.WavesCompleted)
        {
            GameManager.gameManager.CompleteMission();
        }
    }

    public void UpdateEnergyBallUI(int count)
    {
        exteriorHUD.UpdateEnergyBallUI(count);
    }
}
