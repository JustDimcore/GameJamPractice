using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform SpawnPoint;
    public List<MobPath> Paths;

    private void Awake()
    {
        Paths = GetComponentsInChildren<MobPath>().ToList();
    }
}
