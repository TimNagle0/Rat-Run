using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ObstacleBehaviour : MonoBehaviour
{

    private class ObstacleSection
    {
        public GameObject section;
        public bool hasColor;
        private MeshRenderer meshRenderer;
        private Color color;

        public ObstacleSection(GameObject s)
        {
            section = s;
            hasColor = false;
            meshRenderer = section.GetComponent<MeshRenderer>();
        }

        public void SetColor(Color c, string name)
        {
            section.tag = name;
            meshRenderer.material.color = c;
            hasColor = true;
        }


    }
    public float angularMovementSpeed;
    public float angularVariance;
    private bool canMove = false;
    private int movementDirection = 0;

    private string sectionColor1 = "";
    private string sectionColor2 = "";
    [SerializeField] private List<GameObject> rings = new List<GameObject>();
    Dictionary<string, Color> obstacleColors = new Dictionary<string, Color>();
    string[] keys = new string[7];

    private List<ObstacleSection> sections = new List<ObstacleSection>();

    public event Action<int, string, string> sectionInfo;

    public void ObstacleSetup(int ring, bool move, bool moveRight, float speed, float variance)
    {
        CreateColors();
        GameObject currentRing = Instantiate(rings[ring], transform);
        MeshRenderer[] currentSections = currentRing.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer s in currentSections)
        {
            sections.Add(new ObstacleSection(s.gameObject));
        }

        SetStartingSections();
        ColorSections();


        canMove = move;
        if (moveRight)
        {
            movementDirection = 1;
        }
        else
        {
            movementDirection = -1;
        }
        angularMovementSpeed = UnityEngine.Random.Range(speed - variance, speed + variance);
    }

    private void SetStartingSections()
    {
        List<string> colors = new List<string>() { "red", "green", "blue" };
        int spot_1 = UnityEngine.Random.Range(0, sections.Count);
        int spot_2 = UnityEngine.Random.Range(0, sections.Count);
        if (spot_1 == spot_2 && spot_1 != 0)
        {
            spot_2 = 0;
        }else if (spot_1 == spot_2)
        {
            spot_2 = 1;
        }
        sectionColor1 = colors[UnityEngine.Random.Range(0, 3)];
        sectionColor2 = colors[UnityEngine.Random.Range(0, 3)];
        sections[spot_1].SetColor(obstacleColors[sectionColor1], sectionColor1);
        sections[spot_2].SetColor(obstacleColors[sectionColor2], sectionColor2);
    }

    public class SectionInfo {
        public int speed;
        public string color1;
        public string color2;

    }


    public SectionInfo GetSectionInfo()
    {
        SectionInfo s = new SectionInfo();
        s.speed = (int) angularMovementSpeed * movementDirection;
        s.color1 = sectionColor1;
        s.color2 = sectionColor2;
        return s;
        
    }

    private void ColorSections()
    {
        foreach(ObstacleSection os in sections)
        {
            if (!os.hasColor)
            {
                string color = keys[UnityEngine.Random.Range(0, 7)];
                os.SetColor(obstacleColors[color], color);
            }
        }
    }

    private void CreateColors()
    {
        Color orange = new Color(1, 0.5f, 0);
        Color pink = new Color(1, 0.25f, 0.75f);
        obstacleColors.Add("red", Color.red);
        obstacleColors.Add("green", Color.green);
        obstacleColors.Add("blue", Color.blue);
        obstacleColors.Add("yellow", Color.yellow);
        obstacleColors.Add("magenta", Color.magenta);
        obstacleColors.Add("orange", orange);
        obstacleColors.Add("pink", pink);
        obstacleColors.Keys.CopyTo(keys,0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;


        float angle = movementDirection * angularMovementSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angle);
    }


}
