using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    // The speed at which the player moves across the screen; vertical and horizontal can vary
    [SerializeField] float xMoveSpeed = 1f;
    [SerializeField] float yMoveSpeed = 1f;

    // The max offset from the (0, 0) displacement from the camera (local transform) the ship can move; screen borders
    [SerializeField] float maxXRange = 1f;
    [SerializeField] float maxYRange = 1f;

    [Header("Rotation")]
    // Factor to multiply the position by to get the pitch angle; used to keep the ship facing forward so you don't always shoot center screen
    [SerializeField] float posPitchFactor = -5f;
    [SerializeField] float posYawFactor = 5f;

    // Factor to multiply the throw by to get the additional angle correction while moving; basically, nose goes more up when moving up and vice versa
    [SerializeField] float controlPitchFactor = -5f;
    [SerializeField] float controlYawFactor = -5f;
    [SerializeField] float controlRollFactor = -5f;

    // The current "throw" of the vertical and horizontal axes of input, from 0 to 1; used for calculation in multiple methods
    float horizontalThrow = 0f;
    float verticalThrow = 0f;
    bool canMove = true;


    void Update()
    {
        if (canMove)
        {
            ProcessMovementInput();
            ProcessRotation();
        }
    }

    // Processes the player's movement; clamps to screen size (set in inspector) and is framerate independent
    private void ProcessMovementInput()
    {
        horizontalThrow = Input.GetAxis("Horizontal");
        verticalThrow = Input.GetAxis("Vertical");

        float xOffset = horizontalThrow * xMoveSpeed * Time.deltaTime;
        float yOffset = verticalThrow * yMoveSpeed * Time.deltaTime;

        float rawNewXPos = transform.localPosition.x + xOffset;
        float rawNewYPos = transform.localPosition.y + yOffset;

        float newXPos = Mathf.Clamp(rawNewXPos, -maxXRange, maxXRange);
        float newYPos = Mathf.Clamp(rawNewYPos, -maxYRange, maxYRange);

        transform.localPosition = new Vector3(newXPos, newYPos, transform.localPosition.z); // localPosition is the position relative to the parent/what's seen in inspector
    }

    // Automatically rotates the ship when flying around the screen so the nose is always (visually) facing forward, rather than the center of the screen
    // This makes it so when firing, the ship will fire where it is facing, rather than the center of the screen only
    private void ProcessRotation()
    {
        // Coupled to position on screen
        float pitchCorrectionDueToPositon = transform.localPosition.y * posPitchFactor;
        // Coupled to control throw
        float pitchCorrectionDueToControl = verticalThrow * controlPitchFactor;
        float pitch = pitchCorrectionDueToPositon + pitchCorrectionDueToControl;

        // Coupled to position on screen
        float yawCorrectionDueToPosition = transform.localPosition.x * posYawFactor;
        // Coupled to control throw
        float yawCorrectionDueToControl = horizontalThrow * controlYawFactor;
        float yaw = yawCorrectionDueToPosition + yawCorrectionDueToControl;

        // Coupled to control throw
        float roll = horizontalThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void OnPlayerDeath()
    { // Called by string reference, make sure to change in CollisionHandler if name changed
        canMove = false;
    }
}
