using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExteriorManager : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {

    }

    public void HandleShipDestroyed(Ship ship)
    {
        // TODO: Handle game over
        Debug.Log("ExteriorManager -> Game Over!");
    }
}
