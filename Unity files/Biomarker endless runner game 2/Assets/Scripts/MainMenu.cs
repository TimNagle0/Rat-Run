using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject credits;
    private void Start()
    {
        Invoke("DisableCredits", 2f);
    }


   void DisableCredits()
   {
        credits.SetActive(false);
   }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ShowLevelSelection()
    {

    }
}
