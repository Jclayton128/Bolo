using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    //init
    Slider energyBar;
    

    //param
    public float maxEnergy { get; private set; } = 100;
    public float energyGainRate { get; private set; } = 10f; //energy per second

    //hood
    float currentEnergy;

    void Start()
    {
        currentEnergy = maxEnergy;
        energyBar = FindObjectOfType<UIManager>().GetEnergyBar(transform.root.gameObject);
        UpdateUI();

    }

    public void Reinitialize()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateEnergy();    
    }

    private void RegenerateEnergy()
    {
        currentEnergy += energyGainRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!energyBar) { return; }
        energyBar.value = currentEnergy;
    }

    public void ModifyCurrentEnergy(float value)
    {
        currentEnergy += value;
        UpdateUI();
    }

    public void ModifyMaxEnergy(float value)
    {
        maxEnergy += value;
        energyBar.maxValue = maxEnergy;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }


}
