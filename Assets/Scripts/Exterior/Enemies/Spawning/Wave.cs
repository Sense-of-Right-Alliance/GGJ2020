using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Wave
{
    public string Name = "Untitled Wave";

    public List<WaveEvent> WaveEvents;
    /*
    [SerializeField] List<WaveEvent> _waveEvents;
    public List<WaveEvent> WaveEvents { get { return _waveEvents; } }

    */
    public Wave(List<WaveEvent> waveEvents)
    {
        WaveEvents = waveEvents;
    }
}
