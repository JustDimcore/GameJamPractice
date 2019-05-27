using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public float ChangeDirectionDistance;
    
    private int _pointIndex = 0;
    private List<Vector3> _path;
    private bool _pathDone;
    private bool _initialized;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Animator>().SetFloat("MoveSpeed", 1);
    }

    public void Move(List<Vector3> path)
    {
        _path = path;
        Agent.SetDestination(_path[0]);
        _initialized = true;
    }

    private void FixedUpdate()
    {
        if (!_initialized)
            return;

        var targetRotation = Quaternion.LookRotation(_path[_pointIndex] - _rigidbody.position);
        _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, 180 * Time.fixedDeltaTime);
        
        if (!_pathDone && IsDestinationReached())
        {
            Debug.Log("Destination reached: " + _pointIndex);
            if (_path == null || _path == null)
            {
                Debug.LogError("Empty path");
            }
            if (_pointIndex < _path.Count - 1)
            {
                _pointIndex++;
                Agent.SetDestination(_path[_pointIndex]);
            }
            else
            {
                _pathDone = true;
                GameController.Instance.OnMobExit(this);
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
        Debug.Log("Reached trigger");
    }
}
