using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : ControlSource
{
    //init
    public GameObject shiftKnob;
    [SerializeField] Transform[] gearShiftPositions = null;


    //param

    //hood
    public bool LMBdown = false;
    public bool RMBdown = false;
    public Vector3 mousePos = new Vector3(0, 0, 0);

    private void Awake()
    {
        int amCount = FindObjectsOfType<AllegianceManager>().Length;
        if (amCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    protected override void Start()
    {
        base.Start();
        shiftKnob = GameObject.FindGameObjectWithTag("ShiftKnob");
        gearShiftPositions[0] = GameObject.FindGameObjectWithTag("ShiftPos1").transform;
        gearShiftPositions[1] = GameObject.FindGameObjectWithTag("ShiftPos2").transform;
        gearShiftPositions[2] = GameObject.FindGameObjectWithTag("ShiftPos3").transform;

    }

    // Update is called once per frame
    protected override void Update()
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
        HandleGearShifting();

    }

    private void HandleGearShifting()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {            
            speedSetting++;
            //TODO: Play an audioclip with gear shifting 'clunk'
            if (speedSetting > gearShiftPositions.Length)
            {
                speedSetting = 1;
            }
        }

        shiftKnob.transform.position = gearShiftPositions[speedSetting-1].transform.position;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Scan()
    {
        //players have to do their own scanning...
    }

}
