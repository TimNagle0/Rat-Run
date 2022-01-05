using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TunnelManager : MonoBehaviour
{
    [SerializeField] GameObject TunnelSection;
    [SerializeField] GameObject Obstacle;
    [SerializeField] GameObject Finish;
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


    public event Action sendNewSectionInfo;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Entrance")
        {
            currentSection = other.transform.parent.gameObject;
            SetCurrentSectionInfo();
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

    public void CreateFinish()
    {
        Vector3 finishPosition = nextTunnelPosition += Vector3.forward * (0.5f * sectionOffset);
        Instantiate(Finish, finishPosition, Quaternion.identity);
    }

    private void SetCurrentSectionInfo()
    {
        currentSection.GetComponentInChildren<ObstacleBehaviour>().SetSectionInfo();
        sendNewSectionInfo();
    }




}
