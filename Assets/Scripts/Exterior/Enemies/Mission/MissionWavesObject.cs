using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MissionWavesObject", menuName = "Waves", order = 1)]
public class MissionWavesObject : ScriptableObject
{
    public string missionName = "New Mission";
    public Wave[] waves;
}