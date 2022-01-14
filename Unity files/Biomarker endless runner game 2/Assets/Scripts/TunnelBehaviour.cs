using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform obstacleLocation;
    [SerializeField] private GameObject obstacle;
    private GameObject currentObstacle;


    //Movedirection is determined by 0: right, 0.5: random, 1: left
    public void ObstacleSetup(int sectionAmount, int turningChance, float moveDirection, bool randomizeSpeed)
    {
        int ring = sectionAmount - 3; //just to access the ring in the list without an if statement
        transform.Rotate(0, 0, Random.Range(0, 360));

        currentObstacle = Instantiate(obstacle, obstacleLocation);
        int tc = Random.Range(1, 3);
        bool canMove = tc <= turningChance;
        ObstacleBehaviour ob = currentObstacle.GetComponent<ObstacleBehaviour>();
        ob.ObstacleSetup(ring,canMove, Random.value > moveDirection,50,20);
    }

}
