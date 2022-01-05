using UnityEngine;
using System.Collections;

public class GameMusicPlayer : MonoBehaviour
{

    private static GameMusicPlayer instance = null;
    private AudioSource audioSource;
    public static GameMusicPlayer Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
