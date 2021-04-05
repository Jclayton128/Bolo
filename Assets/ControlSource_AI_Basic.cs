using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlSource_AI_Basic : ControlSource
{
    //init

    NavMeshAgent nma;

    //param

    //hood
    public Vector3 navTarget_current;
    public  Vector3 nextSteeringPoint;
    protected override void Start()
    {
        base.Start();

        nma = GetComponent<NavMeshAgent>();
        nma.updateRotation = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SetNavTarget();
        GenerateControlVectorToDriveTowardsNavTarget();
        transform.position = GetComponentInChildren<Transform>().position;
    }

    private void SetNavTarget()
    {
        navTarget_current = targetGO.transform.position;               
    }
    private void GenerateControlVectorToDriveTowardsNavTarget()
    {
        nma.SetDestination(navTarget_current);
        nextSteeringPoint = nma.steeringTarget;
        Vector2 commandedVector = nextSteeringPoint - transform.position;
        float distToNavTarget = (transform.position - navTarget_current).magnitude;
        if (distToNavTarget > 1)
        {
            commandedVector.Normalize();
        }
        horizComponent = commandedVector.x;
        vertComponent = commandedVector.y;


        Debug.DrawLine(transform.position, transform.position + new Vector3(horizComponent, vertComponent, 0), Color.green);

        DebugDrawPath(nma.path.corners);
    }

    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        int i = 0;
        for (; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
        }
        Debug.DrawLine(corners[0], corners[1], Color.red);
    }


}
