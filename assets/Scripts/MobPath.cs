using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobPath : MonoBehaviour
{
    public List<Transform> Waypoints;

    void Awake()
    {
        if (Waypoints == null || Waypoints.Count == 0)
        {
            Waypoints = GetComponentsInChildren<Transform>().Where(t => t != transform).ToList();
        }
    }
}
