using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : ControlSource
{
    //init

    //param

    //hood
    public bool LMBdown = false;
    public bool RMBdown = false;
    public Vector3 mousePos = new Vector3(0, 0, 0);

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            LMBdown = true;
            attack.AttackCommence();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            LMBdown = false;
            attack.AttackRelease();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RMBdown = true;
            //attack.RMBDown();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            RMBdown = false;
            //attack.RMBUp();
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
    }

    private void HandleKeyboardInput()
    {
        horizComponent = Input.GetAxis("Horizontal");
        vertComponent = Input.GetAxis("Vertical");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
