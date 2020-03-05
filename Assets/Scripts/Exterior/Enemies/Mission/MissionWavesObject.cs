using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MissionWavesObject", menuName = "MissionWavesObject", order = 1)]
public class MissionWavesObject : ScriptableObject
{
    public string MissionName = "New Mission";
    public List<Wave> Waves;
}