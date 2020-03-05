using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidMissionDetails : MissionDetails
{
    public void Awake()
    {
        _name = "Asteroid Belt";

        _description = "Navigate through a dangerous asteroid belt, guarded by pirates and aliens alike.";

        _waves = new List<Wave>
        {
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV, SpawnZone.Bottom)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabRight, SpawnPattern.Column, SpawnZone.Right)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV, SpawnZone.Bottom)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabLeft, SpawnPattern.Column, SpawnZone.Left)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.CrabRight, SpawnPattern.Column, SpawnZone.Right)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Random)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingV, SpawnZone.Bottom)),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.Speedster, SpawnPattern.FlyingVInverted, SpawnZone.Bottom)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Center, SpawnZone.Bottom)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingVInverted)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.Center)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.Column)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.FlyingV)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HomingCorvet, SpawnPattern.Center)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.NovaSaucer, SpawnPattern.Center)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Column)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.NovaSaucer, SpawnPattern.Column)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.FlyingV)),
            })

        };
    }
}
