using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MeatResources;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Camera")]
    public Camera LevelCamera;

    [Header("Preset fields")]
    public List<Blender> Blenders;
    public int MobsLimit = 1;
    public float MobSpawningInterval = 1;

    [Header("Mobs")]
    public GameObject MobPrefab;
    public GameObject WaypointsHolder;
    public GameObject MobSpawnersHolder;
    private readonly List<MobController> _mobs = new List<MobController>();
    private Coroutine _spawnCoroutine;
    private Vector3[] _waypoints;
    private Vector3[] _mobSpawners;

    [Header("Players")]
    [Range(1,4)] public int PlayersCount;
    public GameObject PlayerPrefab;
    public List<Transform> PlayersSpawnPoints;
    private readonly List<PlayerControl> _players = new List<PlayerControl>();

    [Header("Meats")]
    public Transform MeatsParent;
    public GameObject MeatPrefab;
    [Range(1, 10)] public int MeatsCount;
    [Range(1, 3)] public float MeatDistance;
    [Range(1, 10)] public float MeatForce;
    private readonly List<Meat> _meats = new List<Meat>();

    [Header("Laser")] 
    public Laser[] Lasers;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Restart();
    }

    public void Restart()
    {
        ClearOldGame();
        StartNewGame();
    }

    private void ClearOldGame()
    {
        Players.Clear(_players);

        _mobs.Clear();
        if(_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        Meats.Clear(_meats);

        if (Lasers != null)
            foreach (var laser in Lasers)
                laser.gameObject.SetActive(false);

        // TODO: Clear statistics
    }

    private void StartNewGame()
    {
        FillWaypoints();
        
        Players.Spawn(PlayersCount, PlayerPrefab, PlayersSpawnPoints, _players);

        // Mobs spawning
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        
        // TODO: Start laser spawning
        if (Lasers != null)
            foreach (var laser in Lasers)
                laser.gameObject.SetActive(true);
    }

    private void FillWaypoints()
    {
        _waypoints = WaypointsHolder
            .GetComponentsInChildren<Transform>()
            .Where(t => t != WaypointsHolder.transform)
            .Select(t => t.position)
            .ToArray();
        
        _mobSpawners = MobSpawnersHolder
            .GetComponentsInChildren<Transform>()
            .Where(t => t != MobSpawnersHolder.transform)
            .Select(t => t.position)
            .ToArray();
    }
    
    public void AddMeat()
    {
        Meats.Spawn(MeatsCount, MeatPrefab, MeatsParent, _mobs, _meats);
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (_mobs.Count < MobsLimit)
            {
                SpawnMob();
            }
            yield return new WaitForSeconds(MobSpawningInterval);
        }
    }

    [ContextMenu("SpawnMob")]
    private void SpawnMob()
    {
        var go = Instantiate(MobPrefab);
        var mob = go.GetComponent<MobController>();
        _mobs.Add(mob);

        var spawnerIndex = Random.Range(0, _mobSpawners.Length);
        var spawner = _mobSpawners[spawnerIndex];
        mob.Agent.Warp(spawner);
        var path = new List<Vector3>();

        for (var i = 0; i < Random.Range(3, 10); i++)
        {
            Vector3 point;
            do
            {
                point = _waypoints[Random.Range(0, _waypoints.Length)];
            } while (path.Contains(point));
            path.Add(point);
        }
        // Add exit point
        path.Add(_mobSpawners[Random.Range(0, _mobSpawners.Length)]);
        
        mob.Move(path);
    }

    // Remove mob when he comes to a door
    public void OnMobExit(MobController mob)
    {
        _mobs.Remove(mob);
        Destroy(mob.gameObject);
    }
}
