using UnityEngine;
using TMPro;
using System;

public class PlayerInput : ControlSource
{
    //init
    public UIManager uim;
    public GameObject shiftKnob;
    public Transform[] gearShiftPositions = null;
    TextMeshProUGUI followMeText;


    //param

    //hood
    public bool LMBdown = false;
    public bool RMBdown = false;
    public Vector3 mousePos = new Vector3(0, 0, 0);
    bool isFollowMeOn = true;

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
        followMeText = uim.GetFollowMeText(gameObject);
    }

    public void ReinitializePlayer()
    {
        Start();
        GetComponentInChildren<Health>().Reinitialize();
        GetComponentInChildren<MoneyHolder>().Reinitialize();
        GetComponentInChildren<HouseHolder>().Reinitialize();
        GetComponentInChildren<Energy>().Reinitialize();
        GetComponentInChildren<CaptureTool>().Reinitialize();
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
        if (Input.GetKeyUp(KeyCode.F))
        {
            isFollowMeOn = !isFollowMeOn;
            UpdateFollowMeLightUI();
        }
        HandleGearShifting();

    }

    private void UpdateFollowMeLightUI()
    {
        if (isFollowMeOn)
        {
            followMeText.color = new Color(1, 1, 0, 1);
        }
        if (!isFollowMeOn)
        {
            followMeText.color = new Color(1, 1, 0, 0.2f);
        }
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

        if (!shiftKnob) { return; }
        shiftKnob.transform.position = gearShiftPositions[speedSetting - 1].transform.position;
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
