using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;

    private void Start()
    {
        // TODO: Init agent params
    }

    public void Move(NavMeshPath path)
    {
        Agent.SetPath(path);
        // Agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Process door trigger
    }
}
