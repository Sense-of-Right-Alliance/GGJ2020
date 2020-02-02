using System;
using System.Collections.Generic;
using System.Linq;

public class WaveEvent
{
    public float Duration { get; private set; }

    public Squadron Squadron { get; private set; }

    private static WaveEvent Delay(float duration)
    {
        return new WaveEvent
        {
            Duration = duration
        };
    }

    public static WaveEvent ShortDelay() => Delay(2f);

    public static WaveEvent MediumDelay() => Delay(5f);

    public static WaveEvent LongDelay() => Delay(10f);

    public static WaveEvent SpawnSquadron(Squadron squadron)
    {
        return new WaveEvent
        {
            Squadron = squadron
        };
    }
}
