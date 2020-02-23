using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorProblem : MonoBehaviour
{
    public UnityGameObjectEvent ProblemDestroyedOrRemovedEvent;

    private void Start()
    {
        if (ProblemDestroyedOrRemovedEvent == null) ProblemDestroyedOrRemovedEvent = new UnityGameObjectEvent();
    }

    private void Update()
    {
        
    }

    public void HandleDestroyedOrRemoved()
    {
        ProblemDestroyedOrRemovedEvent.Invoke(gameObject);
    }
}
