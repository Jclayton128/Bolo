using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation_AI : MonoBehaviour
{
    //init
    GameObject targetGO;
    ControlSource cs;
    [SerializeField] GameObject weaponProjectile = null;
    Rigidbody2D rb;
    Attack attack;



    //param
    public float rotationSpeed = 360; //deg per second
    public float weaponSpeed = 10;
    public float weaponLifetime = 0.3f;
    float acceptableAngleOffBoresight = 0.1f;
    

    //hood
    float distToTargetGO;
    Vector3 dirToTargetGO;
    float attackRange;
       
    void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        attackRange = weaponLifetime * weaponSpeed;
        rb = GetComponentInParent<Rigidbody2D>();
        attack = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        targetGO = cs.GetTargetObject();
        CalculateDistanceToTargetGO();
        //FireWeapon();
    }

    void FixedUpdate()
    {
        TurnToFaceTargetGO();
    }

    private void TurnToFaceTargetGO()
    {
        Vector3 dir = Vector3.zero;
        float ang = 0;
        if (distToTargetGO > 2* attackRange)
        {
            Vector3 vel = rb.velocity;
            dir = (vel - transform.position).normalized;
            ang = Vector3.SignedAngle(transform.up, dir, Vector3.forward);
            Debug.Log("outside range, " + ang);
        }
        if (distToTargetGO <= 2 * attackRange)
        {
            dir = (cs.GetTargetObject().transform.position - transform.position).normalized;
            Debug.DrawLine(transform.position, transform.position + dir, Color.red);
            ang = Vector3.SignedAngle(transform.up, dir, Vector3.forward);
            Debug.Log("within range, " + ang);
        }

        if (ang > acceptableAngleOffBoresight)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        if (ang < -acceptableAngleOffBoresight)
        {
            transform.Rotate(Vector3.forward, -1 * rotationSpeed * Time.deltaTime);
        }
    }
    private void CalculateDistanceToTargetGO()
    {
        distToTargetGO = (cs.GetTargetObject().transform.position - transform.position).magnitude;
    }
}
