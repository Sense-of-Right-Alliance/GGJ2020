using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
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
        Debug.Log("ExteriorManager -> Game Over!");
        SceneManager.LoadScene(0);
    }
}
