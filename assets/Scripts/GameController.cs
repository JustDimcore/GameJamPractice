using System.Collections;
using System.Collections.Generic;
using MeatResources;
using Player;
using UnityEngine;
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
    public List<Spawner> MobSpawners;
    private readonly List<MobController> _mobs = new List<MobController>();

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

    private Coroutine _spawnCoroutine;

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

        // TODO: Remove mobs
        Meats.Clear(_meats);
        // TODO: Disable laser
        // TODO: Clear statistics
    }

    private void StartNewGame()
    {
        Players.Spawn(PlayersCount, PlayerPrefab, PlayersSpawnPoints, _players);

        // TODO: Start mobs spawning
        
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        // TODO: Start laser spawning
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

    private void RecreateCharacter(int playerIndex)
    {
        // TODO: Remove old/disable controlling
        // TODO: Create new
    }

    [ContextMenu("SpawnMob")]
    private void SpawnMob()
    {
        var go = Instantiate(MobPrefab);
        var mob = go.GetComponent<MobController>();
        _mobs.Add(mob);

        var spawnerIndex = Random.Range(0, MobSpawners.Count);
        var spawner = MobSpawners[spawnerIndex];
        var pathIndex = Random.Range(0, spawner.Paths.Count);
        var path = spawner.Paths[pathIndex];
        mob.Agent.Warp(spawner.SpawnPoint.position);
        mob.Move(path);
    }

    // Remove mob when he comes to a door
    public void OnMobExit(MobController mob)
    {
        // TODO: Remove character, which comes to door
        _mobs.Remove(mob);
        Destroy(mob.gameObject);

        // TODO: Create new one in another
        // TODO: Maybe wait some time before spawn
    }
}
