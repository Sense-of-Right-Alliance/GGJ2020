using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WaveEvent
{
    [SerializeField] float _duration = 0f;
    public float Duration { get { return _duration; } private set { _duration = value; } }

    [SerializeField] SpawnPattern _pattern;
    [SerializeField] SpawnZone _zone;
    [SerializeField] EnemyType _enemyType;
    [SerializeField] int _count;

    private Squadron _squadron;
    public Squadron Squadron {
        get
        {
            if (_squadron == null) _squadron = new Squadron(_enemyType, _pattern, _zone, _count);
            return _squadron;
        }
    }

    private static WaveEvent Delay(float duration)
    {
        return new WaveEvent
        {
            _duration = duration
        };
    }

    public static WaveEvent ShortDelay() => Delay(2f);

    public static WaveEvent MediumDelay() => Delay(5f);

    public static WaveEvent LongDelay() => Delay(10f);

    public static WaveEvent SpawnSquadron(Squadron squadron)
    {
        return new WaveEvent
        {
            _squadron = squadron
        };
    }
}
