using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public List<Transform> MobSpawnPoints;
    public List<Transform> PlayersSpawnPoints;
    public List<Blender> Blenders;

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

    private void SpawnMob()
    {
        // TODO: Create path
        // TODO: Create mob
    }
    
    // Remove when he comes to door
    private void OnMobExit()
    {
        // TODO: Remove character, which comes to door
        // TODO: Create new one in another
    }
}
