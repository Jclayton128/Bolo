using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Soldier : Movement
{
    //init

    //param

    //hood
    public Vector3 commandedVector = new Vector3();

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateCurrentMoveSpeed();
    }

    private void FixedUpdate()
    {
        commandedVector.x = cs.horizComponent;
        commandedVector.y = cs.vertComponent;
        WalkInCommandedVector();
        TurnToCommandedVector();

    }

    private void TurnToCommandedVector()
    {
        if (!isCommandedToMove) { return; }
        Vector3 lookDir = (transform.position - cs.facingTargetPoint );
        float ang = Vector3.SignedAngle(transform.up, lookDir, Vector3.forward);

        if (ang < -0.1f)
        {
            rb.angularVelocity = turnSpeed_normal;
        }
        if (ang > 0.1f)
        {
            rb.angularVelocity = -turnSpeed_normal;
        }

    }

    private void WalkInCommandedVector()
    {
        if (!isCommandedToMove) { return; }
        rb.velocity = commandedVector * moveSpeed_current;
    }
}
