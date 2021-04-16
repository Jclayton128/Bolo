using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Soldier : Movement
{
    //init

    //param


    //hood
    public Vector3 commandedVector = new Vector3();
    float maxAngleOffBoresightToDrive = 10f;
    float angleOffCommandedVector;

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
        //turn towards steering target instantly
        //walk towards steering target
        commandedVector.x = cs.horizComponent;
        commandedVector.y = cs.vertComponent;
        rb.velocity = commandedVector * moveSpeed_current;
    }
}
