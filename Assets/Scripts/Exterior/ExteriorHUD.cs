using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ExteriorHUD : MonoBehaviour
{
    [SerializeField] GameObject energyBallList;

    [SerializeField] GameObject energyBallPrefab;

    List<GameObject> _energyBalls = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < energyBallList.transform.childCount; i++)
        {
            GameObject ball = energyBallList.transform.GetChild(i).gameObject;
            _energyBalls.Add(ball);
        }
        
        for (int i = 0; i < _energyBalls.Count; i++)
        {
            _energyBalls[i].transform.SetParent(null);
            _energyBalls[i].SetActive(false);
        }
    }

    private void Update()
    {
        
    }

    public void UpdateEnergyBallUI(int count)
    {
        for (int i = 0; i < _energyBalls.Count; i++)
        {
            _energyBalls[i].transform.SetParent(null);
            _energyBalls[i].SetActive(false);
        }

        for (int i = 0; i < count;i++)
        {
            if (i >= _energyBalls.Count)
            {
                _energyBalls.Add(Instantiate(energyBallPrefab));
            }

            _energyBalls[i].SetActive(true);
            _energyBalls[i].transform.SetParent(energyBallList.transform);
        }
    }
}