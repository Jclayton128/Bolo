﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[RequireComponent (typeof(IFF))]

public class CitySquare : MonoBehaviour
{
    //init
    IFF iff;
    SpriteRenderer sr;
    TextMeshProUGUI cityNameTextBar;
    Slider cityCaptureSlider;
    GameObject player;
    IFF playerIFF;


    //param
    float cityRadius = 5f;
    float timeToCapture = 15; //seconds
    float captureRange = 0.5f;

    //hood
    string cityName;
    List<IFF> buildingInCity = new List<IFF>();
    float timeSpentCapturing = 0;

    void Start()
    {
        SetupCityCaptureSlider();
        player = Finder.FindNearestGameObjectWithTag(transform, "Player");
        playerIFF = player.GetComponent<IFF>();
        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
        iff = GetComponentInChildren<IFF>();
        SelectCityName();
        FindHousesWithinCityAndAdjustAllegiance();
    }

    private void SetupCityCaptureSlider()
    {
        cityCaptureSlider = GameObject.FindGameObjectWithTag("CCB").GetComponent<Slider>();
        cityCaptureSlider.maxValue = timeToCapture;
        cityCaptureSlider.minValue = 0;
        cityCaptureSlider.value = 0;
    }

    private void SelectCityName()
    {
        CityNamesManager cnm = FindObjectOfType<CityNamesManager>();
        cityName = cnm.GetRandomCityName();
    }
    private void FindHousesWithinCityAndAdjustAllegiance()
    {
        List<GameObject> allGOs = new List<GameObject>();
        allGOs = Finder.FindAllGameObjectsWithinSearchRange(transform, cityRadius);
        foreach (GameObject possibleBuilding in allGOs)
        {
            if (possibleBuilding.TryGetComponent(out IFF possIFF))
            {
                int currentAllegiance = iff.GetIFFAllegiance();
                possIFF.SetIFFAllegiance(currentAllegiance);
                buildingInCity.Add(possIFF);
                continue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleCaptureAttempts();
        CheckIfCaptured();
        //CheckForHousesRemaining();
        DisplayCityNameIfWithinRange();
    }

    private void CheckIfCaptured()
    {
        if (timeSpentCapturing >= timeToCapture)
        {
            int newAllegiance = playerIFF.GetIFFAllegiance();
            iff.SetIFFAllegiance(newAllegiance);
            foreach (IFF buildingIFF in buildingInCity)
            {
                buildingIFF.SetIFFAllegiance(newAllegiance);
            }
            timeSpentCapturing = 0;
        }
    }

    private void HandleCaptureAttempts()
    {
        if (playerIFF.GetIFFAllegiance() == iff.GetIFFAllegiance()) { return; }
        if ((player.transform.position - transform.position).magnitude <= captureRange)
        {
            timeSpentCapturing += Time.deltaTime;
        }
        else
        {
            timeSpentCapturing -= Time.deltaTime * 2;
            timeSpentCapturing = Mathf.Clamp(timeSpentCapturing, 0, timeToCapture);
        }
        cityCaptureSlider.value = timeSpentCapturing;
    }

    private void DisplayCityNameIfWithinRange()
    {
        float dist = (player.transform.position - transform.position).magnitude;
        if (dist <= cityRadius)
        {
            cityNameTextBar.text = cityName;
            cityCaptureSlider.value = timeSpentCapturing;
        }
        else
        {
            cityNameTextBar.text = " ";
            cityCaptureSlider.value = 0;
        }
    }

    private void CheckForHousesRemaining()
    {
        if (buildingInCity.Count <= 0)
        {
            sr.color = Color.red;
        }
    }

    public void RemoveHouseFromList(IFF deadHouse)
    {
        buildingInCity.Remove(deadHouse);
    }
}