using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    //init

    //param
    public float moveSpeed = 1.0f;
    public float turnSpeed = 360f; //deg/sec

    //hood
    public Vector3 commandedVector = new Vector3();
    float maxAngleOffBoresightToDrive = 10f;
    bool isCommandedToMove = false;
    float angleOffCommandedVector;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, commandedVector.normalized + transform.position, Color.blue);
        CheckForCommandedMovement();
        UpdateCommandedVectorAndAngleOffIt();
        RotateToCommandedVector();
        DriveAlongCommandedVector();
    }

    private void CheckForCommandedMovement()
    {
        if (Mathf.Abs(cs.horizComponent) > 0 || Mathf.Abs(cs.vertComponent) > 0) //if move commands are non-zero;
        {
            isCommandedToMove = true;
        }
        if (Mathf.Abs(cs.horizComponent) == 0f && Mathf.Abs(cs.vertComponent) == 0f) //if move commands are non-zero;
        {
            isCommandedToMove = false;
        }
    }

    private void DriveAlongCommandedVector()
    {
        if (!isCommandedToMove || Mathf.Abs(angleOffCommandedVector) > maxAngleOffBoresightToDrive)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime*3);
            return;
        }
        if (isCommandedToMove)
        {
            if (Mathf.Abs(angleOffCommandedVector) < maxAngleOffBoresightToDrive * 4)
            {
                rb.velocity = commandedVector * (moveSpeed / 2);
            }
            if (Mathf.Abs(angleOffCommandedVector) < maxAngleOffBoresightToDrive)
            {
                rb.velocity = commandedVector * moveSpeed;
            }
        }
    }

    private void UpdateCommandedVectorAndAngleOffIt()
    {
        commandedVector.x = cs.horizComponent;
        commandedVector.y = cs.vertComponent;
        commandedVector.Normalize();
        angleOffCommandedVector = Vector3.SignedAngle(transform.up, commandedVector, Vector3.forward);
    }

    private void RotateToCommandedVector()
    {
        if (!isCommandedToMove)
        {
            rb.angularVelocity = 0;
            return;
        }
        if (angleOffCommandedVector > -0.1f)
        {
            rb.angularVelocity = turnSpeed;
        }
        if (angleOffCommandedVector < 0.1f)
        {
            rb.angularVelocity = -turnSpeed;
        }
    }
}
