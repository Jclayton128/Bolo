using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : ControlSource
{
    //init

    //param


    //hood
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
        horizComponent = Input.GetAxis("Horizontal");
        vertComponent = Input.GetAxis("Vertical");
    }
}
