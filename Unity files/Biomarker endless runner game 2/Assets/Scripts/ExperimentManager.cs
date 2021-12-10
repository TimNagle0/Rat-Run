using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class ExperimentManager : MonoBehaviour
{
    //BOF STUFF
    // Imports of JS functions defined in the .jslib in Plugins folder 
    [DllImport("__Internal")]
    public static extern void RedirectBOF();
    [DllImport("__Internal")]
    public static extern void RequestConfigFromJS();

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private TunnelManager tunnelManager;
    [SerializeField] private LevelManager levelManager;

    private UIController uiController;

    #region variables
    private string startTimeExperiment = "";
    private int currentLevel;
    private int currentTarget;
    private int targetRotationSpeed;
    private string currentTargetColor1 = "";
    private string currentTargetColor2 = "";

    private float maxExperimentTime = 300f;
    private bool experimentCanEnd = false;
    #endregion

    #region Events
    private void SubscribeToEvents()
    {
        
        playerBehaviour.changeColor += ColorChange;
        playerInput.movement += Movement;
        playerBehaviour.increaseScore += TargetPassed;
        playerBehaviour.takeDamage += TargetFailed;
        playerBehaviour.changeColor += ColorChange;
        tunnelManager.sectionInfo += TargetInfo;
        levelManager.endLevel += LoadLevel;
        
    }
    private void LoadLevel(int level, bool isLast)
    {
        PrepareSummaryDataPoint(level, isLast);
    }
    private void ColorChange(string oldColor, string newColor)
    {
        PrepareContinuousDataPoint("colorChange", oldColor, newColor);
    }

    private void Movement(string direction)
    {
        PrepareContinuousDataPoint(direction);
    }

    private void TargetPassed()
    {
        PrepareContinuousDataPoint("targetPassed");
    }
    private void TargetFailed()
    {
        PrepareContinuousDataPoint("targetFailed");
    }

    private void TargetInfo(int speed, string color1, string color2)
    {
        currentTarget++;
        targetRotationSpeed = speed;
        currentTargetColor1 = color1;
        currentTargetColor2 = color2;
        PrepareContinuousDataPoint("targetInfo");
    }

    #endregion
    #region retrieving player data


    private int GetCurrentSpeed()
    {
        return (int)playerInput.forwardMovementSpeed;
    }

    private string GetCurrentColor()
    {
        return playerBehaviour.gameObject.tag;
    }
    #endregion

    #region Data Classes
    public class ContinuousData
    {
        public int level { get; set; }
        public string timeStamp { get; set; }
        public string dataType { get; set; }
        public int currentTarget { get; set; }
        public int targetRotationSpeed { get; set; }
        public string targetColor1 { get; set; }
        public string targetColor2 { get; set; }
        public int currentSpeed { get; set; }
        public string currentColor { get; set; }
        public string newColor { get; set; }
        public ContinuousData(int currentLevel, int target, string color1, string color2)
        {
            level = currentLevel;
            currentTarget = target;
            targetColor1 = color1;
            targetColor2 = color2;
            newColor = "";
            timeStamp = System.DateTime.Now.ToString();
        }
    }

    public class SummaryData
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int level { get; set; }
        public int totalTime { get; set; }
        public int totalScore { get; set; }
        public int totalDistance { get; set; }
        public int totalPresses { get; set; }
        public int totalDirectionChanges { get; set; }
        public int totalColorChanges { get; set; }

        public int falseColorChanges { get; set; }
        public int falseKeypresses { get; set; }

        public SummaryData(string start, int currentLevel)
        {
            startTime = start;
            level = currentLevel;
            endTime = System.DateTime.Now.ToString();
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("UI").GetComponent<UIController>();
        startTimeExperiment = System.DateTime.Now.ToString();
        currentLevel = SceneManager.GetActiveScene().buildIndex - 2;
        SubscribeToEvents();
    }

    private void Update()
    {
        if (Time.time > maxExperimentTime)
        {
            uiController.end.gameObject.SetActive(true);
            experimentCanEnd = true;
        }

        if (experimentCanEnd)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PrepareSummaryDataPoint(0, true);
            }
        }
    }

    #region Datapoint methods

    public void PrepareContinuousDataPoint(string category)
    {

        ContinuousData dataPoint = new ContinuousData(currentLevel, currentTarget, currentTargetColor1, currentTargetColor2);
        dataPoint.dataType = category;
        dataPoint.currentSpeed = GetCurrentSpeed();
        dataPoint.currentColor = GetCurrentColor();

        SendFilesContinuous(dataPoint);
    }

    public void PrepareContinuousDataPoint(string category, string oldColor, string newColor)
    {

        ContinuousData dataPoint = new ContinuousData(currentLevel, currentTarget, currentTargetColor1, currentTargetColor2);
        dataPoint.dataType = category;
        dataPoint.currentSpeed = GetCurrentSpeed();
        dataPoint.currentColor = oldColor;
        dataPoint.newColor = newColor;

        SendFilesContinuous(dataPoint);
    }


    public void PrepareSummaryDataPoint(int level, bool isLast)
    {

        SummaryData dataPoint = new SummaryData(startTimeExperiment, currentLevel);

        dataPoint.totalTime = (int)Time.timeSinceLevelLoad;
        dataPoint.totalScore = PlayerStats.totalColorChanges;
        dataPoint.totalDistance = PlayerStats.totalDistanceMoved;
        dataPoint.totalPresses = PlayerStats.totalKeypresses;
        dataPoint.totalDirectionChanges = PlayerStats.totalDirectionChanges;
        dataPoint.totalColorChanges = PlayerStats.totalColorChanges;
        dataPoint.falseColorChanges = PlayerStats.falseColorChanges;
        dataPoint.falseKeypresses = PlayerStats.falseKeyPresses;

        SendFilesSummary(dataPoint, level, isLast);
    }

    public void SendFilesContinuous(ContinuousData dataPoint)
    {
        WWWForm form;
        form = AddFieldsContinuous(dataPoint);
        StartCoroutine(SendFiles(form, false, 0, false));

    }

    public void SendFilesSummary(SummaryData dataPoint, int level, bool isLast)
    {

        WWWForm form;
        form = AddFieldsSummary(dataPoint);
        StartCoroutine(SendFiles(form, true, level, isLast));



    }

    private WWWForm AddFieldsContinuous(ContinuousData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("formType", "Continuous");
        form.AddField("level", data.level);
        form.AddField("timeStamp", data.timeStamp);
        form.AddField("dataType", data.dataType);
        form.AddField("currentTarget", data.currentTarget);
        form.AddField("rotationSpeed", data.targetRotationSpeed);
        form.AddField("targetColor1", data.targetColor1);
        form.AddField("targetColor2", data.targetColor2);
        form.AddField("currentSpeed", data.currentSpeed);
        form.AddField("currentColor", data.currentColor);
        form.AddField("newColor", data.newColor);
        return form;
    }

    private WWWForm AddFieldsSummary(SummaryData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("formType", "Summary");
        form.AddField("startTime", data.startTime);
        form.AddField("endTime", data.endTime);
        form.AddField("level", data.level);
        form.AddField("totalTime", data.totalTime);
        form.AddField("totalScore", data.totalScore);
        form.AddField("totalDistance", data.totalDistance);
        form.AddField("totalKeyPresses", data.totalPresses);
        form.AddField("totalDirectionChanges", data.totalDirectionChanges);
        form.AddField("totalColorChanges", data.totalColorChanges);
        form.AddField("falseColorChanges", data.falseColorChanges);
        form.AddField("falseKeyPresses", data.falseKeypresses);

        return form;
    }

    #endregion

    public IEnumerator SendFiles(WWWForm form, bool loadNext, int level, bool isFinalForm)
    {
        // Instead of the URL we can use # to get to the same route as the game was delivered on
        // Alternatively specify the URL of the server with port and route
        // var url = "http://127.0.0.1:5000/game"
        var url = "#";

        UnityWebRequest UWRPost = UnityWebRequest.Post(url, form);
        yield return UWRPost.SendWebRequest();

        if (UWRPost.isNetworkError || UWRPost.isHttpError)
        {
            Debug.Log(UWRPost.error);
        }
        UWRPost.Dispose();
        if (isFinalForm)
        {
            Invoke("StopExperiment", 1f);
        }
        else if (loadNext)
        {

            StartCoroutine(LoadNextScene(level, 1f));
        }
    }

    public void StopExperiment()
    {
        RedirectBOF();
    }

    private IEnumerator LoadNextScene(int scene, float delay)
    {

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
