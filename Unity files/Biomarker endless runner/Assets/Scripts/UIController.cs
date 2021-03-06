using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // UI Manager class that controls which UI elements should be shown / updated


    [SerializeField] PlayerBehaviour player;
    public UILives lives;
    public Text Score;
    public GameObject GameOverScreen;
    public Text Level;
    public Text end;
    // Start is called before the first frame update
    void Start()
    {
        Level.text = (SceneManager.GetActiveScene().buildIndex - 2).ToString();
        GameOverScreen.SetActive(false);
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
        if(PlayerStats.lives == 0)
        {
            GameOverScreen.SetActive(true);
        }

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
