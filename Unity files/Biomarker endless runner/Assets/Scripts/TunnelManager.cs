using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TunnelManager : MonoBehaviour
{
    [SerializeField] GameObject TunnelSection;
    [SerializeField] GameObject Obstacle;
    [SerializeField][Range(0, 3)] private int turningChance;
    [SerializeField][Range(0, 3)] private int colorChangeChance;

    public bool canSpawn = true;
    private GameObject lastSection;
    private GameObject currentSection;

    private Vector3 nextTunnelPosition;
    private Vector3 startPosition = new Vector3(0, 0, 0);
    private int sectionOffset = 20;



    List<GameObject> Sections = new List<GameObject>();

    public event Action<string, string> sectionInfo;


    // Create the first two tunnel sections
    void Start()
    {
        CreateNewSection();
        nextTunnelPosition = startPosition + sectionOffset*Vector3.forward;
        CreateNewSection();

    }


    // If the player enters a new section, update all sections
    // Also detects if the upcoming target should change color
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entrance")
        {
            currentSection = other.transform.parent.gameObject;
            SendSectionInfo(currentSection);
            UpdateSections();
        }else if (other.tag == "ColorChange")
        {
            currentSection.GetComponent<TunnelBehaviour>().SetRandomColor(colorChangeChance);
        }
            

        
    }


    //Function that calls an event with info about the current target
    // Meant for data collection only atm
    private void SendSectionInfo(GameObject section)
    {
        TunnelBehaviour tb = section.GetComponent<TunnelBehaviour>();
        string direction = "";
        string targetColor = tb.currentObstacle.tag;
        
        if (tb.moveRight)
        {
            direction = "right";
        }
        else
        {
            direction = "left";
        }
        sectionInfo(direction, targetColor);
    }

    // returns the current rotation of an obstacle
    public float GetTargetRotation()
    {
        if(currentSection == null)
        {
            return 0;
        }
        TunnelBehaviour tb = currentSection.GetComponent<TunnelBehaviour>();
        if(tb.currentObstacle == null)
        {
            return 0;
        }
        float rotation = tb.currentObstacle.transform.rotation.z;
        return rotation;
    }
    

    // Destroy the previous section and create a new one at the end of the tunnel
    private void UpdateSections()
    {
        if(lastSection != null)
        {
            Destroy(lastSection);
        }

        lastSection = currentSection;
        nextTunnelPosition += Vector3.forward * sectionOffset;
        CreateNewSection();
    }


    // Check if sections are allowed to spawn and create a new one
    private void CreateNewSection()
    {
        if (!canSpawn)
            return;
        GameObject newSection = GameObject.Instantiate(TunnelSection, nextTunnelPosition, Quaternion.identity);
        newSection.GetComponent<TunnelBehaviour>().ObstacleSetup(turningChance);
    }




}
