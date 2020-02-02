using System;
using System.Collections.Generic;
using System.Linq;

public class Wave
{
    public List<WaveEvent> WaveEvents { get; }

    public Wave(List<WaveEvent> waveEvents)
    {
        WaveEvents = waveEvents;
    }
}
