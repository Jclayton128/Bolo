using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement class is the base class that all unit movements derive from;

    //init
    protected ControlSource cs;
    protected Attack attack;
    protected Rigidbody2D rb;

    //param
    public float moveSpeed_normal;
    public float turnSpeed_normal; //deg/sec

    //hood
    protected float moveSpeed_current;
    protected float turnSpeed_current;
    protected bool isCommandedToMove = false;

    protected virtual void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        attack = transform.parent.GetComponentInChildren<Attack>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckForCommandedMovement();
    }
    protected virtual void CheckForCommandedMovement()
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

    protected virtual void UpdateCurrentMoveSpeed()
    {
        //TODO: Get and use terrain speed modifier
        float gearModifier = (cs.speedSetting) / 2f;
        moveSpeed_current = moveSpeed_normal * gearModifier;
        //Debug.Log("CS speed setting: " + cs.speedSetting +  ". gear mod: " + gearModifier + " . MSC: " + moveSpeed_current);

    }
}
