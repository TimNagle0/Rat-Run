using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    //Simple data class with all the playerdata for the experiment / game
    public static int lives;
    public static int currentLevel;
    public static int currentScore;
    public static int totalColorChanges;
    public static int falseColorChanges;
    public static int totalDistanceMoved;
    public static int totalKeypresses;
    public static int falseKeyPresses;
    public static int totalDirectionChanges;
    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        currentScore = 0;
        currentLevel = SceneManager.GetActiveScene().buildIndex - 2;
        totalColorChanges = 0;
        falseColorChanges = 0;
        totalDistanceMoved = 0;
        totalKeypresses = 0;
        falseKeyPresses = 0;
        totalDirectionChanges = 0;
    }
}
