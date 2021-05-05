using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSourceSoldier : ControlSource
{
    //init
    public GameObject factionLeader;
    public CitySquare homeCity;
    public CitySquare targetCity;
    public GameObject closestEnemyTurret;
    public GameObject closestEnemyUnit;
    public GameObject playerToFollow;
    StealthSeeker ss;

    //param
    float attackRange;
    float offsetPointAmount = 1.0f;

    //hood
    Vector3 navTarget;
    Vector3 nextSteeringPoint;
    int ownAllegiance; //this is fine to set because combat units don't just change allegiance...

    protected override void Start()
    {
        base.Start();
        ownAllegiance = iff.GetIFFAllegiance();
        attackRange = attack.GetAttackRange();
        speedSetting = 2;
        ss = GetComponentInChildren<StealthSeeker>();
        factionLeader = am.factionLeaders[ownAllegiance].gameObject;
    }

    protected override void Update()
    {
        base.Update();
        DecideBehavior();
        //Navigate();

        //UpdateNavTarget();
        //GenerateControlVectorTowardsNavTarget();
    }

    private void DecideBehavior()
    {
        if (closestEnemyUnit || closestEnemyTurret)
        {
            //Attack();
            return;
        }
        if (playerToFollow)
        {
            //FollowPlayer();
            return;
        }
        if (targetCity)
        {
            //InvadeCity();
            return;
        }
        if (homeCity)
        {
            //DefendCity();
            return;
        }
        else
        {
            Debug.Log("No behaviour selected!");
        }

    }



    public override void Scan()
    {
        float scanRange = ss.GetComponent<CircleCollider2D>().radius;
        if (ut.TryGetClosestUnitWithinRange(gameObject, scanRange, "Turret", ownAllegiance, out closestEnemyTurret) == true)
        {
            return;
        }
        if (ut.TryGetClosestAttackerWithinRange(gameObject, scanRange, ownAllegiance, out closestEnemyUnit) == true)
        {
            return;
        }

        float distToFactionLeader = (transform.position - factionLeader.transform.position).magnitude;
        if (distToFactionLeader <= scanRange)
        {
            playerToFollow = factionLeader;
            return;
        }
        if (cm.TryGetCitySquareWithinRange(transform, scanRange, ownAllegiance, out targetCity) == false)
        {
            homeCity = cm.FindNearestCitySquare(transform, ownAllegiance);
        }
        playerToFollow = null;
        closestEnemyTurret = null;
        closestEnemyTurret = null;
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
        if (closestEnemyUnit)
        {
            //move to within 50% attack range of closest attacking unit and face i
            if (TestForLOSForAttack(closestEnemyUnit.transform.position, attackRange * .7f))
            {
                Debug.Log("called for attack");
                attack.AttackCommence();
                speedSetting = 1; //slowdown
                Vector3 dir = (transform.position - closestEnemyUnit.transform.position).normalized;
                navTarget = closestEnemyUnit.transform.position + (dir * attackRange * .5f);
            }
            else
            {
                speedSetting = 2;
                navTarget = closestEnemyUnit.transform.position;
            }

            Debug.Log("closest attacking unit");
            return;
        }
        if (closestEnemyTurret)
        {
            //move to within 50% attack range of closest Defense Turret and face it.
            if (TestForLOSForAttack(closestEnemyTurret.transform.position, attackRange * .5f))
            {
                Debug.Log("called for attack");
                attack.AttackCommence();
                speedSetting = 1; //slowdown
                Vector3 dir = (closestEnemyTurret.transform.position - transform.position).normalized;
                navTarget = transform.position + (dir * 0.5f);
            }
            else
            {
                speedSetting = 2;
                navTarget = closestEnemyTurret.transform.position;
            }
            Debug.Log("closest defense turret");
            speedSetting = 1;
            return;
        }

        if (targetCity && !closestEnemyTurret)
        {
            //move to exactly on target CitySquare.
            navTarget = targetCity.transform.position;
            Debug.Log("capturing city");
            speedSetting = 2;
            return;
        }

       

    }




}
