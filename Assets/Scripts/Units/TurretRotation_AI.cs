using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation_AI : MonoBehaviour
{
    //init
    GameObject targetGO;
    ControlSource cs;
    Rigidbody2D rb;
    Attack attack;



    //param
    public float rotationSpeed = 360; //deg per second
    float acceptableAngleOffBoresight = 5f;
    

    //hood
    float distToTargetGO;
    Vector3 dirToTargetGO = Vector3.zero;
    float ang = 0;
    float attackRange = 0;

    void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        rb = GetComponentInParent<Rigidbody2D>();
        attack = GetComponent<Attack>();
        attackRange = attack.GetAttackRange();
    }

    // Update is called once per frame
    void Update()
    {
        targetGO = cs.GetTargetObject();
        CalculateDistanceToTargetGO();
        FireWeapon();
    }

    private void FireWeapon()
    {
        if (distToTargetGO <= attackRange && Mathf.Abs(ang) <= acceptableAngleOffBoresight)
        {
            Debug.Log("kaboom!");
            attack.AttackCommence();
        }
    }

    void FixedUpdate()
    {
        TurnToFaceTargetGO();
    }

    private void TurnToFaceTargetGO()
    {

        if (distToTargetGO > 2 * attackRange)
        {
            Vector3 vel = rb.velocity;
            dirToTargetGO = (vel - transform.position);
            ang = Vector3.SignedAngle(transform.up, dirToTargetGO, Vector3.forward);
            //Debug.Log("outside range, " + ang);
        }
        if (distToTargetGO <= 2 * attackRange)
        {
            dirToTargetGO = (cs.GetTargetObject().transform.position - transform.position);
            Debug.DrawLine(transform.position, transform.position + dirToTargetGO, Color.red);
            ang = Vector3.SignedAngle(transform.up, dirToTargetGO, Vector3.forward);
            //Debug.Log("within range, " + ang);
        }
        float angClamped = Mathf.Clamp01(Mathf.Abs(ang)/10);
        float currentRotSpeed = rotationSpeed * angClamped * angClamped;
        Debug.Log("ang: " + angClamped + " . CRS: " + currentRotSpeed);

        if (ang > acceptableAngleOffBoresight)
        {
            transform.Rotate(Vector3.forward, currentRotSpeed * Time.deltaTime);
        }
        if (ang < -acceptableAngleOffBoresight)
        {
            transform.Rotate(Vector3.forward, -1 * currentRotSpeed * Time.deltaTime);
        }

    }

    private void CalculateDistanceToTargetGO()
    {
        distToTargetGO = (cs.GetTargetObject().transform.position - transform.position).magnitude;
    }
}
