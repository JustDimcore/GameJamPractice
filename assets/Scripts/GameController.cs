using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Preset fields")]
    public List<Spawner> MobSpawners;
    public List<Transform> PlayersSpawnPoints;
    public List<Blender> Blenders;

    [Header("Prefabs")]
    public GameObject MobPrefab;
    public GameObject PlayerPrefab;

    [Header("Runtime fields")]
    // TODO: public List<PlayerController> Players;
    public List<MobController> Mobs;

    [Header("Camera")]
    public Camera LevelCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartNewGame();
    }

    void InitGame()
    {
        ClearOldGame();
        StartNewGame();
    }

    private void ClearOldGame()
    {
        // TODO: Clear old objects and states

        // TODO: Remove characters
        // TODO: Remove mobs
        // TODO: Remove body parts
        // TODO: Disable laser
        // TODO: Clear statistics
    }

    private void StartNewGame()
    {
        // TODO: Create characters
        // TODO: Start mobs spawning
        SpawnMob();
        // TODO: Start laser spawning
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
        Mobs.Add(mob);
        
        var spawnerIndex = Random.Range(0, MobSpawners.Count - 1);
        var spawner = MobSpawners[spawnerIndex];
        var pathIndex = Random.Range(0, spawner.Paths.Count - 1);
        var path = spawner.Paths[pathIndex];
        mob.Agent.Warp(spawner.SpawnPoint.position);
        mob.Move(path);
    }
    
    // Remove mob when he comes to a door
    private void OnMobExit(MobController mob)
    {
        // TODO: Remove character, which comes to door
        Mobs.Remove(mob);
        Destroy(mob.gameObject);
        
        // TODO: Create new one in another
        // TODO: Maybe wait some time before spawn
        SpawnMob();
    }
}
