using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //init
    GameObject playerAtThisComputer;
    public Slider healthBar = null;
    public GameObject shiftKnob = null;
    public Transform shiftPos_1 = null;
    public Transform shiftPos_2 = null;
    public Transform shiftPos_3 = null;
    public TextMeshProUGUI houseCounter = null;


    // Start is called before the first frame update
    void Start()
    {
        playerAtThisComputer = GameObject.FindGameObjectWithTag("Player");  //This is the keystone place where the UI manager assigns the active UI components
        //a particular player.
    }

    // Update is called once per frame
    void Update()
    {

    }
}
