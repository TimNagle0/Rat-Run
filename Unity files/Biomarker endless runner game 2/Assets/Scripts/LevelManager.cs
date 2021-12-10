using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public PlayerInput player;
    public TunnelManager tunnelManager;

    public float LevelTime;
    [Range(1,12)]public float speedMultiplier;
    public float startSpeed;
    public float maxSpeed;
    [Range(3,7)]public int sections;
    [Range(0, 3)] public int turningChance;
    [Header("0 is right, 0.5 is random, 1 is left")]
    [Range(0, 1)] public float movementDirection;
    public bool randomizeTurningSpeed;

    private float startTime;
    private float currentSpeed;
    private bool isRunning;

    public event Action<int, bool> endLevel;
    // Start is called before the first frame update
    void Start()
    {
        startTime = 0;
        isRunning = true;
        player.SetMovementSpeed(startSpeed);
        player.SetMovementSpeed(startSpeed);
        tunnelManager.SetupTunnelManager(sections, turningChance, movementDirection, randomizeTurningSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Mathf.Sqrt(speedMultiplier*Time.time);
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }else if (currentSpeed < startSpeed)
        {
            currentSpeed = startSpeed;
        }
        player.SetMovementSpeed(currentSpeed);

        if(Time.timeSinceLevelLoad > LevelTime && isRunning)
        {
            isRunning = false;
            tunnelManager.canSpawn = false;
            Invoke("LoadNextLevel", 13 - speedMultiplier);
        }
        if (PlayerStats.lives <= 0 && isRunning)
        {
            isRunning = false;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2, 1));
        }
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

    private void LoadNextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        bool isLast = SceneManager.GetSceneByBuildIndex(level).IsValid();

        endLevel(level, isLast);
    }
}
