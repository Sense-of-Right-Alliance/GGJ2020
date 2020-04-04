using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] WaveConfig waveConfig; // overwrites waypoints and movespeed vars

    private List<Transform> Waypoints
    {
        get
        {
            if (waveConfig == null)
            {
                return waypoints;
            }
            else
            {
                return waveConfig.GetWaypoints();
            }
        }
    }

    private float MoveSpeed
    {
        get
        {
            if (waveConfig == null)
            {
                return moveSpeed;
            }
            else
            {
                return waveConfig.GetMoveSpeed();
            }
        }
    }

    int waypointIndex = 0;

    private void Start()
    {
        transform.position = waypoints[waypointIndex].position;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex < Waypoints.Count - 1)
        {
            Vector2 targetPosition = Waypoints[waypointIndex + 1].position;
            float movementThisFrame = MoveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if ((Vector2)transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            CompletePathing();
        }
    }

    private void CompletePathing()
    {
        Destroy(gameObject);
    }
}
