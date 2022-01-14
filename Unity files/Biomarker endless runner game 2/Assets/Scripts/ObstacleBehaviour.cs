using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ObstacleBehaviour : MonoBehaviour
{
    // A class for obstacle sections to set their color and tags
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

    // A list with the different rings of 3-7 sections
    [SerializeField] private List<GameObject> rings = new List<GameObject>();
    // A list with the sections of this obstacles ring
    private List<ObstacleSection> sections = new List<ObstacleSection>();
    Dictionary<string, Color> obstacleColors = new Dictionary<string, Color>();
    string[] keys = new string[7];

    public event Action<int, string, string> sectionInfo;

    public void ObstacleSetup(int ring, bool move, bool moveRight, float speed, float variance)
    {
        CreateColors();
        // Create the ring and add the sections to the section list
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

    // A function that sets 2 random sections of the ring to colors that the player can change into.
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

    // This function colors the rest of the sections, the colors can still be red,green or blue.
    private void ColorSections()
    {
        foreach (ObstacleSection os in sections)
        {
            if (!os.hasColor)
            {
                string color = keys[UnityEngine.Random.Range(0, 7)];
                os.SetColor(obstacleColors[color], color);
            }
        }
    }


    // Sets the variables in the playerstats class to this section
    public void SetSectionInfo()
    {
        float speed = angularMovementSpeed * movementDirection;
        string color1 = sectionColor1;
        string color2 = sectionColor2;
        if (canMove)
        {
            PlayerStats.SetSectionInfo(speed, color1, color2);
        }
        else
        {
            PlayerStats.SetSectionInfo(0, color1, color2);
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


    void Update()
    {
        if (!canMove)
            return;

        float angle = movementDirection * angularMovementSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angle);
    }


}
