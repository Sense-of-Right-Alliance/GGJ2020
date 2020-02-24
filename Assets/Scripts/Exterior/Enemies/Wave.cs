using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Wave
{
    public string name = "Untitled Wave";
    [SerializeField] List<WaveEvent> _waveEvents;

    
    public List<WaveEvent> WaveEvents { get { return _waveEvents; } }

    public Wave(List<WaveEvent> waveEvents)
    {
        _waveEvents = waveEvents;
    }
}
