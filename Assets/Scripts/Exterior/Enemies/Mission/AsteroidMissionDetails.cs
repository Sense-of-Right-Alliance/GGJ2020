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
            /*
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid, 1)),
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),

            }),
            */
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 1)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumShrapnelAsteroid, SpawnPattern.Center)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumShrapnelAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumShrapnelAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 2)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumShrapnelAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 3)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumShrapnelAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 4)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BulletAsteroid, SpawnPattern.Random, SpawnZone.Top, 20, 0.2f)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BulletAsteroid, SpawnPattern.Random, SpawnZone.Top, 20, 0.2f)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BulletAsteroid, SpawnPattern.Random, SpawnZone.Top, 20, 0.2f)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BulletAsteroid, SpawnPattern.Random, SpawnZone.Top, 20, 0.2f)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BulletAsteroid, SpawnPattern.Random, SpawnZone.Top, 20, 0.2f)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.MediumAsteroid, SpawnPattern.JostledRow)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.ResourceAsteroid, SpawnPattern.Random, SpawnZone.Top, 1)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 5)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.HugeAsteroid, SpawnPattern.Center, SpawnZone.TopAsteroid, 1)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.RadialTurret, SpawnPattern.Center, SpawnZone.Top, 1)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.RadialTurret, SpawnPattern.JostledRow, SpawnZone.Top, 2)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 4)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.RadialTurret, SpawnPattern.Random, SpawnZone.Top, 3)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.AsteroidTurret, SpawnPattern.Random, SpawnZone.Top, 4)),
            }),
            new Wave(new List<WaveEvent>
            {
                WaveEvent.LongDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Center)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.FlyingV, SpawnZone.Top, 3)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.Center)),
                WaveEvent.MediumDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.PlainJane, SpawnPattern.FlyingV, SpawnZone.Top, 4)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV, SpawnZone.Top, 3)),
                WaveEvent.ShortDelay(),
                WaveEvent.SpawnSquadron(new Squadron(EnemyType.BigBoi, SpawnPattern.FlyingV, SpawnZone.Top, 3)),
            }),
        };
    }
}
