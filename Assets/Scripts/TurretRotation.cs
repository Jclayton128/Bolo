using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : MonoBehaviour
{
    //init
    ControlSource cs;

    //param
    public float rotationSpeed = 360;

    //hood
    Vector3 dirToMouse = new Vector3(0, 0, 0);
    public float angleAwayFromMouse = 0;
    void Start()
    {
        cs = GetComponentInParent<ControlSource>();
    }

    // Update is called once per frame
    void Update()
    {
        FindAngleFromMousePos();
    }

    private void FixedUpdate()
    {
        RotateTurretToMouseDir();
    }

    private void RotateTurretToMouseDir()
    {
        if (Mathf.Abs(angleAwayFromMouse) <= .1f)
        {
            return;
        }
        if (angleAwayFromMouse > 0.1)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        if (angleAwayFromMouse < -.1)
        {
            transform.Rotate(Vector3.forward, -1 * rotationSpeed * Time.deltaTime);
        }
    }

    private void FindAngleFromMousePos()
    {
        dirToMouse = (cs.mousePos - transform.position).normalized;
        angleAwayFromMouse = Vector3.SignedAngle(transform.up, dirToMouse, Vector3.forward);
        //Debug.Log(angleAwayFromMouse);
    }
}
