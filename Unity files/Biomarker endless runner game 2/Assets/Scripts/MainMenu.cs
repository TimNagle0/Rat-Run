using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ShowLevelSelection()
    {

    }
}
