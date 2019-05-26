﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;

    private int _pointIndex = 0;
    private MobPath _path;

    
    public void Move(MobPath path)
    {
        _path = path;
        Agent.SetDestination(_path.Waypoints[0].position);
    }

    private void FixedUpdate()
    {
        if (IsDestinationReached())
        {
            Debug.Log("Destination reached: " + _pointIndex);
            if (_pointIndex < _path.Waypoints.Count - 1)
            {
                _pointIndex++;
            }
            else
            {
                Debug.Log("Path done");
            }
        }
    }

    private bool IsDestinationReached()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Math.Abs(Agent.velocity.sqrMagnitude) < 0.000001f)
                {
                    // Done
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Process door trigger
    }
}
