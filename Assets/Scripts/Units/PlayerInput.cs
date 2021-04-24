using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : ControlSource
{
    //init
    public UIManager uim;
    public GameObject shiftKnob; 
    public Transform[] gearShiftPositions = null;


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
        uim = FindObjectOfType<UIManager>();
        AssignShiftKnobs();
    }

    private void AssignShiftKnobs()
    {
        shiftKnob = uim.GetShiftKnob(gameObject);
        uim.GetShiftPositions(gameObject, out gearShiftPositions[0], out gearShiftPositions[1], out gearShiftPositions[2]);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
