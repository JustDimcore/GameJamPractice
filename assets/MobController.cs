using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    public NavMeshAgent Agent;


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
