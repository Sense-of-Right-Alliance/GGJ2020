using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Siren : MonoBehaviour
{
    public enum AlertState { None, Yellow, Red }

    [SerializeField] GameObject lights;
    [SerializeField] GameObject filter;

    private AudioSource aSource;

    [SerializeField] AlertState alertState = AlertState.None;
    public AlertState Alert { get { return alertState; } }



    public void SetColor(Color c)
    {
        lights.GetComponent<SpriteRenderer>().color = c;
        filter.GetComponent<SpriteRenderer>().color = c;
    }

    public void SetAlert(AlertState alert)
    {
        alertState = alert;

        switch (alert)
        {
            case (AlertState.None):
                lights.SetActive(false);
                filter.SetActive(false);
                aSource.Stop();
                break;
            case (AlertState.Yellow):
                lights.SetActive(true);
                filter.SetActive(true);

                aSource.pitch = 0.5f;
                aSource.Play();

                SetColor(Color.yellow);
                break;
            case (AlertState.Red):
                lights.SetActive(true);
                filter.SetActive(true);

                aSource.pitch = 1f;
                aSource.Play();

                SetColor(Color.red);
                break;
        }
    }

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();

        Vector2 centeredPos = filter.transform.position;
        centeredPos.y = 0;
        filter.transform.position = centeredPos;

        SetAlert(alertState);
    }
}
