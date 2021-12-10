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
    }

    // Update is called once per frame
    void Update()
    {
        //Update player speed
        currentSpeed = Mathf.Sqrt(speedMultiplier*Time.timeSinceLevelLoad);
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }else if (currentSpeed < startSpeed)
        {
            currentSpeed = startSpeed;
        }
        player.SetMovementSpeed(currentSpeed);


        //Timer for the end of the level 
        //Is offset by multiplier so players spent similar time in space after a level
        if(Time.timeSinceLevelLoad > LevelTime && isRunning)
        {
            isRunning = false;
            tunnelManager.canSpawn = false;
            Invoke("LoadNextLevel", 13 - speedMultiplier);
        }

        //If the player runs out of lives set them back 2 levels
        if(PlayerStats.lives <= 0 && isRunning)
        {
            isRunning = false;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2,1));
        }
    }


    //Load a specific level, or the next one based on level result
    private IEnumerator LoadLevel(int level, float delay)
    {
        if(level < 3)
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
