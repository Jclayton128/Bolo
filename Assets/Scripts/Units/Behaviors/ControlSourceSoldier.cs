using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSourceSoldier : ControlSource
{
    //init
    public GameObject factionLeader;
    ControlSource factionLeaderCS;
    public CitySquare homeCity;
    public CitySquare targetCity;
    public GameObject closestEnemyTurret;
    public GameObject closestEnemyUnit;
    public GameObject playerToFollow;
    StealthSeeker ss;

    //param
    float attackRange;
    float distToChangeSpeedWhenFollowing = 1.0f;

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
        factionLeaderCS = factionLeader.GetComponent<ControlSource>();
    }

    protected override void Update()
    {
        base.Update();
        DecideBehavior();
        GenerateControlVectorTowardsNavTarget();

        //UpdateNavTarget();
        //GenerateControlVectorTowardsNavTarget();
    }

    private void DecideBehavior()
    {
        if (closestEnemyUnit || closestEnemyTurret)
        {
            MoveToAttack();
            Attack();
            return;
        }
        if (playerToFollow)
        {
            FollowPlayer();
            return;
        }
        if (targetCity)
        {
            InvadeCity();
            return;
        }
        if (!homeCity)
        {
            FindClosestHomeCity();
        }
        if (homeCity)
        {
            DefendHomeCity();
            return;
        }
        else
        {
            Debug.Log("No behaviour selected!");
        }

    }

    #region behavior implementation
    private void MoveToAttack()
    {
        if (closestEnemyTurret && !closestEnemyUnit)
        {
            navTarget = closestEnemyTurret.transform.position;
        }

        if (closestEnemyUnit)
        {
            navTarget = closestEnemyUnit.transform.position;
        }

        float distToNavTarget = (transform.position - navTarget).magnitude;
        if (distToNavTarget > attackRange)
        {
            speedSetting = 2;
        }
        if (distToNavTarget < attackRange * 0.75f)
        {
            speedSetting = 1;
        }
        if (distToNavTarget < attackRange * 0.3f)
        {
            navTarget = transform.position;
        }
    }

    private void Attack()
    {
        if (TestForLOSForAttack(closestEnemyUnit.transform.position, attackRange * .7f))
        {
            //Debug.Log("called for attack");
            attack.AttackCommence();
            speedSetting = 1; //slowdown
        }
    }

    private void FollowPlayer()
    {
        navTarget = playerToFollow.transform.position;
        float distToNavTarget = (navTarget - transform.position).magnitude;
        if (distToNavTarget > distToChangeSpeedWhenFollowing)
        {
            speedSetting = 2;
        }
        if (distToNavTarget <= distToChangeSpeedWhenFollowing)
        {
            speedSetting = 1;
        }
        if (distToNavTarget < 0.3f)
        {
            navTarget = transform.position;
        }
    }

    private void InvadeCity()
    {
        navTarget = targetCity.transform.position;
        speedSetting = 2;
    }

    private void FindClosestHomeCity()
    {
        homeCity = cm.FindNearestCitySquare(transform, ownAllegiance);
    }
    private void DefendHomeCity()
    {
        navTarget = homeCity.transform.position;
        speedSetting = 2;
    }

    #endregion

    protected override void Scan()
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
        if (distToFactionLeader <= scanRange  && factionLeaderCS.GetFollowMeStatus())
        {
            playerToFollow = factionLeader;
            return;
        }
        FindClosestHomeCity();
        if (homeCity)
        {
            float distToHome = (homeCity.transform.position - transform.position).magnitude;
            CitySquare possibleTargetCity = cm.FindNearestCitySquare_IgnoreIFF(transform, ownAllegiance);
            float distToInvade = (possibleTargetCity.transform.position - transform.position).magnitude;
            if (distToHome > distToInvade)
            {
                targetCity = possibleTargetCity;
            }
            else
            {
                targetCity = null;
            }
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
}
