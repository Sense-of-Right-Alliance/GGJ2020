using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MissionDetailsObject", menuName = "MissionDetailsObject", order = 1)]
public class MissionDetailsObject : ScriptableObject
{
    public string MissionName = "New Mission";
    [SerializeField] List<Wave> overrideWaves;

    protected List<Wave> _waves;

    public List<Wave> Waves
    {
        get
        {
            return (overrideWaves.Count > 0) ? overrideWaves : _waves;
        }
    }
}