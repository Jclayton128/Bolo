using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //init

    public GameObject playerAtThisComputer;
    [SerializeField] Slider healthBar  = null;
    [SerializeField] GameObject shiftKnob = null;
    [SerializeField] Transform shiftPos_1 = null;
    [SerializeField] Transform shiftPos_2 = null;
    [SerializeField] Transform shiftPos_3 = null;
    [SerializeField] TextMeshProUGUI houseCounter = null;
    [SerializeField] TextMeshProUGUI moneyCounter = null;
    [SerializeField] Image flag = null;
    [SerializeField] Slider energyBar = null;
    [SerializeField] Image weaponIcon = null;


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
    public Slider GetHealthBar(GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return healthBar;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }
    public GameObject GetShiftKnob(GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return shiftKnob;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }
    public void GetShiftPositions(GameObject askingGameObject, out Transform shift1, out Transform shift2, out Transform shift3)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            shift1 = shiftPos_1;
            shift2 = shiftPos_2;
            shift3 = shiftPos_3;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            shift1 = null;
            shift2 = null;
            shift3 = null;
        }
    }

    public TextMeshProUGUI GetHouseCounter (GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return houseCounter;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }

    public TextMeshProUGUI GetMoneyCounter (GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return moneyCounter;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }

    public Image GetFlag (GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return flag;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }

    public Slider GetEnergyBar(GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return energyBar;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }
    public Image GetWeaponIcon(GameObject askingGameObject)
    {
        if (!playerAtThisComputer) { playerAtThisComputer = GameObject.FindGameObjectWithTag("Player"); }
        if (askingGameObject == playerAtThisComputer)
        {
            return weaponIcon;
        }
        else
        {
            //Debug.Log("Asking GO is not the local player! No UI for you!");
            return null;
        }
    }

}
