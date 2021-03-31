using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation_Player : MonoBehaviour
{
    //init
    PlayerInput playerInput;

    //param
    public float rotationSpeed = 360;

    //hood
    Vector3 dirToMouse = new Vector3(0, 0, 0);
    float angleAwayFromMouse = 0;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();  //TODO Why can't I just look for a Control Source? Only compiles if I search for Player Input directly
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
        dirToMouse = (playerInput.mousePos - transform.position).normalized;
        angleAwayFromMouse = Vector3.SignedAngle(transform.up, dirToMouse, Vector3.forward);
        //Debug.Log(angleAwayFromMouse);
    }
}
