using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float angularMovementSpeed;
    private bool canMove = false;
    private int movementDirection = 0;
    // Start is called before the first frame update
    public void ObstacleSetup(bool move, bool moveRight)
    {       
        canMove = move;
        if (moveRight)
        {
            movementDirection = 1;
        }
        else
        {
            movementDirection = -1;
        }
    }

    // If the obstacle can move, update the rotation
    void Update()
    {
        if (!canMove)
            return;

        float angle = movementDirection * angularMovementSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angle);
    }


}
