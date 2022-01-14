using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;

    // Variables for player movement
    public float angularMovementSpeed = 50;
    public float forwardMovementSpeed = 10;
    public float maxMovementSpeed = 18;
    public float movementDirection = 0;

    // Keycodes for changing color
    private KeyCode red = KeyCode.J;
    private KeyCode green = KeyCode.K;
    private KeyCode blue = KeyCode.L;

    // Variables for keeping track of the player location
    private float distanceToCenter;
    private float lastDirection;

    public event Action<string> movement;

    private void Start()
    {
        lastDirection = 0;
        distanceToCenter = Vector3.Distance(transform.position, transform.GetChild(0).position);
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            PlayerStats.totalKeypresses++;
            if (!UpdateColor())
            {
                PlayerStats.falseKeyPresses++;
            }
        }

        RotatePlayer();
        MovePlayer();

    }
    public void SetMovementSpeed(float speed)
    {
        forwardMovementSpeed = speed;
    }

    private bool UpdateColor()
    {
        if (Input.GetKeyDown(red))
        {
            player.ChangeColor("red");
            return true;
        }
        if (Input.GetKeyDown(green))
        {
            player.ChangeColor("green");
            return true;
        }
        if (Input.GetKeyDown(blue))
        {
            player.ChangeColor("blue");
            return true;
        }
        return false;
    }
    private void RotatePlayer()
    {
        // Updating the player location based on input
        movementDirection = Input.GetAxisRaw("Horizontal");
        if (movementDirection > 0 && movementDirection != lastDirection)
        {
            movement("right");
        }
        else if (movementDirection < 0 && movementDirection != lastDirection)
        {
            movement("left");
        }

        // Keeping track of player location and movement for data collection
        if (movementDirection != lastDirection && movementDirection != 0)
        {
            PlayerStats.totalKeypresses++;
        }
        if (HasChangedDirection(movementDirection))
        {
            PlayerStats.totalDirectionChanges++;
        }
        lastDirection = movementDirection;
        float angle = movementDirection * angularMovementSpeed * Time.deltaTime;
        float distanceMoved = Mathf.Deg2Rad * angle * distanceToCenter;
        PlayerStats.totalDistanceMoved += (int)distanceMoved;
        transform.Rotate(0, 0, angle);
    }
    private bool HasChangedDirection(float newDirection)
    {
        return (newDirection > 0 && lastDirection < 0 | newDirection < 0 && lastDirection > 0);
    }

    private void MovePlayer()
    {
        transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
    }
}
