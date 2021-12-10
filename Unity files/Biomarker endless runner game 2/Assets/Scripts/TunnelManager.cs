using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TunnelManager : MonoBehaviour
{
    [SerializeField] GameObject TunnelSection;
    [SerializeField] GameObject Obstacle;
    private int turningChance;
    private bool randomizeTurningSpeed;
    private float direction;
    private int sections = 3;
    public bool canSpawn = true;
    private GameObject lastSection;
    private GameObject currentSection;

    private Vector3 nextTunnelPosition;
    private Vector3 startPosition = new Vector3(0, 0, 0);
    private int sectionOffset = 20;



    List<GameObject> Sections = new List<GameObject>();
    public event Action<int, string, string> sectionInfo;
    public void SetupTunnelManager(int s, int tc, float d, bool rts)
    {
        sections = s;
        turningChance = tc;
        direction = d;
        randomizeTurningSpeed = rts;

        CreateNewSection();
        nextTunnelPosition = startPosition + sectionOffset * Vector3.forward;
        CreateNewSection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entrance")
        {
            currentSection = other.transform.parent.gameObject;
            SendInfo();
            UpdateSections();
        }

    }

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

    private void CreateNewSection()
    {
        if (!canSpawn)
            return;
        GameObject newSection = GameObject.Instantiate(TunnelSection, nextTunnelPosition, Quaternion.identity);
        newSection.GetComponent<TunnelBehaviour>().ObstacleSetup(sections, turningChance, direction, randomizeTurningSpeed);
    }

    private void SendInfo()
    {
        ObstacleBehaviour.SectionInfo s = currentSection.GetComponentInChildren<ObstacleBehaviour>().GetSectionInfo();
        sectionInfo(s.speed, s.color1, s.color2);
    }




}
