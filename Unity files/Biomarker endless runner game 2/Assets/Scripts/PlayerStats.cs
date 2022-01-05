using UnityEngine.SceneManagement;
using UnityEngine;
public class PlayerStats: MonoBehaviour
{
    public static int lives;
    public static int currentLevel;
    public static int currentScore;
    public static int totalColorChanges;
    public static int falseColorChanges;
    public static int totalDistanceMoved;
    public static int totalKeypresses;
    public static int falseKeyPresses;
    public static int totalDirectionChanges;
    public static CurrentSectionInfo currentSectionInfo;

    public struct CurrentSectionInfo
    {
        public float sectionSpeed;
        public string sectionColor1;
        public string sectionColor2;

    }

    // Start is called before the first frame update
    public void Awake()
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
        currentSectionInfo = new CurrentSectionInfo();
    }

    public static void SetSectionInfo(float speed, string color1, string color2)
    {
        currentSectionInfo.sectionSpeed = speed;
        currentSectionInfo.sectionColor1 = color1;
        currentSectionInfo.sectionColor2 = color2;
    }

}
