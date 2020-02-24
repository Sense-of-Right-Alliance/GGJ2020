using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public enum InteriorProblemType { None, Debris, Steam, Flame, Breach }

[Serializable]
public class InteriorProblemOdds
{
    public float nothingOdds = 1f;
    public float debrisOdds = 1f;
    public float steamOdds = 1f;
    public float flameOdds = 1f;
    public float breachOdds = 1f;

    public int numProblems = 1;

    private Dictionary<InteriorProblemType, float> problemOddsDictionary;

    private Dictionary<InteriorProblemType, float> GetProblemOddsDictionary()
    {
        if (problemOddsDictionary == null)
        {
            problemOddsDictionary = new Dictionary<InteriorProblemType, float>
            {
                { InteriorProblemType.None, nothingOdds},
                { InteriorProblemType.Debris, debrisOdds},
                { InteriorProblemType.Steam, steamOdds},
                { InteriorProblemType.Flame, flameOdds},
                { InteriorProblemType.Breach, breachOdds},
            };
        }

        return problemOddsDictionary;
    }

    public float ComputeChanceForProblem(InteriorProblemType type)
    {
        Dictionary<InteriorProblemType, float> dict = GetProblemOddsDictionary();

        float totalOdds = 0f;
        totalOdds += dict[InteriorProblemType.None];
        totalOdds += dict[InteriorProblemType.Debris];
        totalOdds += dict[InteriorProblemType.Steam];
        totalOdds += dict[InteriorProblemType.Flame];
        totalOdds += dict[InteriorProblemType.Breach];

        return dict[type] / totalOdds;
    }

    public float GetProblemOdds(InteriorProblemType type)
    {
        Dictionary<InteriorProblemType, float> dict = GetProblemOddsDictionary();

        return dict[type];
    }

    public static float ComputeChanceForProblemGeneric(InteriorProblemType type)
    {
        float totalOdds = 0f;
        totalOdds += 100f;
        totalOdds += 100f;
        totalOdds += 100f;
        totalOdds += 100f;
        totalOdds += 100f;

        return 100f / totalOdds;
    }
}
