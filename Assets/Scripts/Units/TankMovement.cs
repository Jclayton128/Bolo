using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankMovement : Movement
{
    //init

    //param
    public float moveSpeed_normal;
    public float turnSpeed_normal; //deg/sec

    //hood
    public Vector3 commandedVector = new Vector3();
    float maxAngleOffBoresightToDrive = 10f;
    bool isCommandedToMove = false;
    float angleOffCommandedVector;
    float moveSpeed_current;
    float turnSpeed_current;

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
        //UpdateMoveTurnPerformanceWithCurrentTerrain();
    }
    private void FixedUpdate()
    {
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
                rb.velocity = commandedVector * (moveSpeed_normal / 2);
            }
            if (Mathf.Abs(angleOffCommandedVector) < maxAngleOffBoresightToDrive)
            {
                rb.velocity = commandedVector * moveSpeed_normal;
            }
        }
    }

    private void UpdateCommandedVectorAndAngleOffIt()
    {
        commandedVector.x = cs.horizComponent;
        commandedVector.y = cs.vertComponent;
        if (commandedVector.magnitude > 1)
        {
            commandedVector.Normalize();
        }
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
            rb.angularVelocity = turnSpeed_normal;
        }
        if (angleOffCommandedVector < 0.1f)
        {
            rb.angularVelocity = -turnSpeed_normal;
        }
    }
}
