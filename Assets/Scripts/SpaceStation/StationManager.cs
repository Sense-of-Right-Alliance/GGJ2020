using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class StationManager : MonoBehaviour
{
    public static StationManager stationManager;

    private List<LaunchPad> readyPads = new List<LaunchPad>();

    private int sceneID = -1;

    private void Awake()
    {
        StationManager.stationManager = this;
    }

    public void PadReady(LaunchPad pad)
    {
        readyPads.Add(pad);

        if (readyPads.Count == 2)
        {
            if (readyPads[0].SceneID == readyPads[1].SceneID)
            {
                Debug.Log("Role Strings = " + readyPads[0].Role.ToString() + " and " + readyPads[1].Role.ToString());
                PlayerPrefs.SetInt(readyPads[0].Role.ToString(), (int)readyPads[0].ReadyID); // TODO read this on mission scenes
                PlayerPrefs.SetInt(readyPads[1].Role.ToString(), (int)readyPads[1].ReadyID);

                sceneID = readyPads[0].SceneID;
                GetComponent<SceneTransitionAnimator>().PlayLeaveStationTransition(LoadScene);
            }
        }
    }

    public void PadUnready(LaunchPad pad)
    {
        readyPads.Remove(pad);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneID);
    }
}
