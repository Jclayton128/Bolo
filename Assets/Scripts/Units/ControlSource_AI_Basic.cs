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
    public float scanRange = 3f;
    float timeBetweenScans = 0.2f;

    //hood
    Vector3 navTarget_current;    
    Vector3 nextSteeringPoint;
    float timeSinceLastScan = 0;
    List<GameObject> targets = new List<GameObject>();
    public GameObject tacticalTarget = null;
    public GameObject strategicTarget = null;
    bool isHealthy = true;


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
        Scan();
        UpdateHealthyStatus();
        DetermineTargets();
        GenerateControlVectorToDriveTowardsNavTarget();
        transform.position = GetComponentInChildren<Transform>().position;
    }

    private void UpdateHealthyStatus()
    {
        if (health.currentHealth >= health.startingHealth / 2)
        {
            isHealthy = true;
        }
        else
        {
            isHealthy = false;
        }
    }

    private void Scan()
    {
        timeSinceLastScan -= Time.deltaTime;
        if (timeSinceLastScan <= 0)
        {
            targets = ut.FindTargetsWithinSearchRange(gameObject, scanRange);
            timeSinceLastScan = timeBetweenScans;
        }
    }

    private void DetermineTargets()
    {

        foreach (GameObject eGO in targets)
        {
            if(eGO.GetComponentInChildren<IFF>().GetIFFAllegiance() == iff.GetIFFAllegiance())
            {
                //Debug.Log("case 1");
                strategicTarget = null;
                continue;
            }
            if (eGO.tag == "Player" && isHealthy) //attack player if I am healthy
            {
                //Debug.Log("case 2");
                tacticalTarget = eGO;
                continue;
            }
            if (eGO.tag == "Player" && !isHealthy)
            {
                //Debug.Log("case 3");
                tacticalTarget = eGO;
                // strategicTarget = an allied base in order to heal
                continue;
            }
            if (eGO.tag == "Turret")
            {
                //Debug.Log("case 4");
                tacticalTarget = eGO;
                continue;
            }
        }

        if (!strategicTarget)
        {
            Debug.Log("case 5");
            CitySquare targetCS;
            if (cm.TryFindNearestCitySquare(transform, iff.GetIFFAllegiance(), out targetCS))
            {
                strategicTarget = targetCS.gameObject;
            }
            else
            {
                //Debug.Log("nowhere really to drive to");
            }
        }
        
        if (strategicTarget)
        {
            navTarget_current = strategicTarget.transform.position;
        }
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

    public override GameObject GetTargetObject()
    {
        GameObject go = null;
        if (tacticalTarget)
        {
            go = tacticalTarget;
        }
        return go;
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
