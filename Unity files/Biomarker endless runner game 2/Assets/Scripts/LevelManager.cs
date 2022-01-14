using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public PlayerInput player;
    public TunnelManager tunnelManager;
    private PlayerBehaviour playerBehaviour;

    [Header("Time in seconds that the level should last")]
    public float LevelTime;

    [Header("Controls how fast the player speed multiplies")]
    [Range(1,12)]public float speedMultiplier;


    public float startSpeed;
    public float maxSpeed;

    [Header("Control the amount of sections in each wheel and it's turning chance")]
    [Range(3,7)]public int sections;
    [Range(0, 3)] public int turningChance;
    [Header("0 is right, 0.5 is random, 1 is left")]
    [Range(0, 1)] public float movementDirection;
    public bool randomizeTurningSpeed;

    private float startTime;
    private float currentSpeed;
    private bool isRunning;

    public event Action<int, bool> endLevel;


    void Start()
    {
        playerBehaviour = player.GetComponentInChildren<PlayerBehaviour>();
        startTime = 0;
        isRunning = true;
        player.SetMovementSpeed(startSpeed);
        playerBehaviour.finishLevel += LoadNextLevelInSeconds;
        tunnelManager.SetupTunnelManager(sections, turningChance, movementDirection, randomizeTurningSpeed);
        
    }

    void Update()
    {
        UpdatePlayerSpeed();

        // Stops the spawning of tunnel sections and creates a finish trigger at the last section
        if (Time.timeSinceLevelLoad > LevelTime && isRunning)
        {
            isRunning = false;
            tunnelManager.canSpawn = false;
            tunnelManager.CreateFinish();
        }

        // Checks if the player has run out of lives and sets them back 2 levels
        if (PlayerStats.lives <= 0 && isRunning)
        {
            isRunning = false;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2, 0.5f));
        }
    }

    // Setting the speed of the player based on the multiplier and time spent in the level
    private void UpdatePlayerSpeed()
    {
        currentSpeed = Mathf.Clamp(Mathf.Sqrt(speedMultiplier * Time.timeSinceLevelLoad),startSpeed,maxSpeed);
        player.SetMovementSpeed(currentSpeed);
    }

    private IEnumerator LoadLevel(int level, float delay)
    {
        if (level < 3)
        {
            level = 3;
        }
        yield return new WaitForSeconds(delay);
        endLevel(level, false);
    }

    private void LoadNextLevelInSeconds(float seconds)
    {
        Invoke("LoadNextLevel", seconds);
    }

    // Checks if there is a next level to be loaded, otherwise ends the game
    private void LoadNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        bool isLast = SceneManager.GetSceneByBuildIndex(level).IsValid();

        endLevel(level, isLast);
    }
}
