using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    //init

    //param
    public float moveSpeed = 1.0f;
    public float turnSpeed = 30f; //deg per second


    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        RespondToMovementCommand();
    }

    private void RespondToMovementCommand()
    {
        rb.velocity = new Vector2(cs.horizComponent, cs.vertComponent) * moveSpeed;
    }
}
