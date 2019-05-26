using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    
    
    private void Awake()
    {
        Instance = this;
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
        
        var path = new NavMeshPath();
        // TODO: Fill path
        
        mob.Move(path);
        
        // TODO: Create path
        // TODO: Create mob
    }
    
    // Remove when he comes to door
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
