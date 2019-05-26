using System;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public float ChangeDirectionDistance;
    
    private int _pointIndex = 0;
    private MobPath _path;
    private bool _pathDone;

    
    public void Move(MobPath path)
    {
        _path = path;
        Agent.SetDestination(_path.Waypoints[0].position);
    }

    private void FixedUpdate()
    {
        if (!_pathDone || IsDestinationReached())
        {
            Debug.Log("Destination reached: " + _pointIndex);
            if (_pointIndex < _path.Waypoints.Count - 1)
            {
                _pointIndex++;
                Agent.SetDestination(_path.Waypoints[_pointIndex].position);
            }
            else
            {
                _pathDone = true;
                Debug.Log("Path done");
            }
        }
    }

    private bool IsDestinationReached()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= ChangeDirectionDistance || Agent.remainingDistance <= Agent.stoppingDistance)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Process door trigger
    }
}
