using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public float ChangeDirectionDistance;
    
    private int _pointIndex = 0;
    private MobPath _path;
    private bool _pathDone;
    private bool _initialized;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Animator>().SetFloat("MoveSpeed", 1);
    }

    public void Move(MobPath path)
    {
        _path = path;
        Agent.SetDestination(_path.Waypoints[0].position);
        _initialized = true;
    }

    private void FixedUpdate()
    {
        if (!_initialized)
            return;

        var targetRotation = Quaternion.LookRotation(_path.Waypoints[_pointIndex].position - _rigidbody.position);
        _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, 180 * Time.fixedDeltaTime);
        
        if (!_pathDone && IsDestinationReached())
        {
            Debug.Log("Destination reached: " + _pointIndex);
            if (_path == null || _path.Waypoints == null)
            {
                Debug.LogError("Empty path");
            }
            if (_pointIndex < _path.Waypoints.Count - 1)
            {
                _pointIndex++;
                Agent.SetDestination(_path.Waypoints[_pointIndex].position);
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
