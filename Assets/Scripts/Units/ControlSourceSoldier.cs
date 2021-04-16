﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSourceSoldier : ControlSource
{
    //init
    public GameObject factionLeader;
    public CitySquare homeCity;
    public CitySquare targetCity;
    public GameObject closestDefenseTurret;
    public GameObject closestAttackingUnit;

    //param
    float attackRange;
    float offsetPointAmount = 1.0f;

    //hood
    Vector3 navTarget;
    Vector3 nextSteeringPoint;
    public bool isInvader = false;

    protected override void Start()
    {
        base.Start();
        DetermineMissionAtStart();
        attackRange = attack.GetAttackRange();
    }

    private void DetermineMissionAtStart()
    {
        int iffAlleg = iff.GetIFFAllegiance();
        Transform factionLeaderTransform = am.GetFactionLeader(iffAlleg).transform;
        CitySquare closestAlliedCS = cm.FindNearestCitySquare(factionLeaderTransform, iffAlleg);

        CitySquare closestCSOfAll = cm.FindNearestCitySquare(factionLeaderTransform);

        if (closestAlliedCS == closestCSOfAll)
        {
            isInvader = false;
            homeCity = closestAlliedCS;
            //faction leader is in an allied region.
            //while in sight of leader, follow him.
            //if lose sight of leader, return to spawned City and Patrol it.
        }
        else
        {
            isInvader = true;
            targetCity = closestCSOfAll;
            //faction leader is in an enemy region.
            //move towards that enemy region's city square          

        }

        //regardless of initial mission:
        //if the region's city square is in sight, determine if guarded (defense turrets within range of City Square).
        //if not guarded, move towards city square
        //if guarded, move towards and attack defense turrets within 'range' of the City Square

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateNavTarget();
        GenerateControlVectorTowardsNavTarget();
    }

    private void GenerateControlVectorTowardsNavTarget()
    {
        nma.SetDestination(navTarget);
        nextSteeringPoint = nma.steeringTarget;

        Vector2 commandedVector = nextSteeringPoint - transform.position;
        float distToNavTarget = (transform.position - navTarget).magnitude;
        if (distToNavTarget > 1)
        {
            commandedVector.Normalize();
        }
        horizComponent = commandedVector.x;
        vertComponent = commandedVector.y;


        Debug.DrawLine(transform.position, transform.position + new Vector3(horizComponent, vertComponent, 0), Color.green);

        DebugDrawPath(nma.path.corners);

    }

    private void UpdateNavTarget()
    {
        if (closestAttackingUnit)
        {
            //move to within 50% attack range of closest attacking unit and face it
            navTarget = closestAttackingUnit.transform.position;
            return;
        }
        if (isInvader && closestDefenseTurret)
        {
            //move to within 50% attack range of closest Defense Turret and face it.
            navTarget = closestDefenseTurret.transform.position;
            return;
        }

        if (isInvader && !closestDefenseTurret)
        {
            //move to exactly on target CitySquare.
            navTarget = targetCity.transform.position;
            return;
        }
        if (!isInvader && factionLeader)
        {
            //move to a point nearby leader via CUR picker.
            if ((factionLeader.transform.position - navTarget).magnitude > offsetPointAmount)
            {
                Vector3 centerPos = factionLeader.transform.position;
                navTarget = CUR.CreateRandomPointNearInputPoint(centerPos, 1.0f, 0.5f);
            }
            return;
        }
        if (!isInvader && !factionLeader)
        {
            //move to a point nearby home City Square.
            if ((homeCity.transform.position - navTarget).magnitude > offsetPointAmount)
            {
                Vector3 centerPos = homeCity.transform.position;
                navTarget = CUR.CreateRandomPointNearInputPoint(centerPos, 1.0f, 0.5f);
            }
            return;
        }

    }

    protected override void Scan()
    {
        if (!isInvader)
        {
            ut.TryGetClosestUnitWithinRange(gameObject, scanRange, iff.GetIFFAllegiance(), out factionLeader);
        }
        if (isInvader)
        {
           if (targetCity.transform.root.GetComponentInChildren<IFF>().GetIFFAllegiance() == iff.GetIFFAllegiance())
            {
                isInvader = false;
                homeCity = targetCity;
                targetCity = null;
            }
        }

        ut.TryGetClosestUnitWithinRange(gameObject, scanRange, "Turret", out closestDefenseTurret);
        closestAttackingUnit = ut.FindClosestTargetWithinSearchRange(gameObject, scanRange, iff.GetIFFAllegiance());
    }



}
