using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform obstacleLocation;
    [SerializeField] private GameObject obstacle;
    Dictionary<string, Color> obstacleColors = new Dictionary<string, Color>();
    string[] keys = new string[3];
    // private Color[] obstacleColors = new Color[3];
    public GameObject currentObstacle;
    private MeshRenderer meshRenderer;

    public bool moveRight;

    private void Start()
    {
        if(currentObstacle == null)
        {
            currentObstacle = Instantiate(obstacle, obstacleLocation, false);
        }

        if(meshRenderer == null)
        {
            meshRenderer = currentObstacle.transform.GetChild(0).GetComponent<MeshRenderer>();
        }
    }

    public void ObstacleSetup(int turningChance)
    {
        // Add the colors for possible targets
        obstacleColors.Add("red", Color.red);
        obstacleColors.Add("green", Color.green);
        obstacleColors.Add("blue", Color.blue);

        // Add a random rotation to the obstacle
        transform.Rotate(0, 0, Random.Range(0, 360));

        // Create the obstacle and get the renderer component for the target
        currentObstacle = Instantiate(obstacle, obstacleLocation);
        meshRenderer = currentObstacle.transform.GetChild(0).GetComponent<MeshRenderer>();
        
        // Set the target to a random color
        obstacleColors.Keys.CopyTo(keys, 0);
        currentObstacle.tag = keys[Random.Range(0, 3)];
        meshRenderer.material.color = obstacleColors[currentObstacle.tag];


        // Check if the obstacle should be turning based on level settings and determine direction randomly
        int tc = Random.Range(1, 3);
        bool canMove = tc <= turningChance;
        if (canMove)
        {
            moveRight = Random.value >= 0.5f;
        }
        currentObstacle.GetComponent<ObstacleBehaviour>().ObstacleSetup(canMove, moveRight);
    }

    // Set the target to a random color other than the one it currently has
    public void SetRandomColor(int changeChance)
    {
        int cc = Random.Range(1, 3);
        if (cc <= changeChance)
        {
            int newColor = Random.Range(0, 3);

            if(currentObstacle.tag == keys[newColor])
            {
                // If the target has the same color as the newly picked one, call this function again
                SetRandomColor(changeChance);
            }
            else
            {
                currentObstacle.tag = keys[newColor];
                meshRenderer.material.color = obstacleColors[currentObstacle.tag];
            }
            
        }
        
    }

}
