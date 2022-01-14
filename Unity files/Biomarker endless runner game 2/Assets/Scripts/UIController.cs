using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    public UILives lives;
    public Text Score;
    public Text Level;
    public Text end;

    [SerializeField]
    Text levelPerm;

    void Start()
    {
        Level.text = string.Format("Level {0}", (SceneManager.GetActiveScene().buildIndex - 2));
        levelPerm.text = string.Format("Level: {0}", (SceneManager.GetActiveScene().buildIndex - 2));
        Invoke("DisableLevelText", 2f);

        player.takeDamage += UpdateLives;
        player.increaseScore += UpdateScore;
    }

    private void DisableLevelText()
    {
        Level.gameObject.SetActive(false);
    }
    private void UpdateScore()
    {
        player.score += 1;
        Score.text = "Score: " + player.score;
    }

    private void UpdateLives()
    {
        lives.TakeDamage();

    }
}
