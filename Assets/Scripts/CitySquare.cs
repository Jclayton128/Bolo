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


    GameObject player;
    IFF playerIFF;


    //param
    public float cityRadius = 5f;
    public float timeToCapture = 15; //seconds
    float captureRange = 0.5f;

    //hood
    public string cityName;
    public List<IFF> buildingsInCity = new List<IFF>();
    public float timeSpentCapturing = 0;

    void Start()
    {

        player = Finder.FindNearestGameObjectWithTag(transform, "Player");
        playerIFF = player.GetComponent<IFF>();

        sr = GetComponent<SpriteRenderer>();
        iff = GetComponentInChildren<IFF>();
        SelectCityName();
        FindHousesWithinCityAndAdjustAllegiance();
    }


    private void SelectCityName()
    {
        CityManager cnm = FindObjectOfType<CityManager>();
        cityName = cnm.GetRandomCityName();
    }
    private void FindHousesWithinCityAndAdjustAllegiance()
    {
        List<GameObject> allGOs = new List<GameObject>();
        allGOs = Finder.FindAllGameObjectsWithinSearchRange(transform, cityRadius);
        foreach (GameObject possibleBuilding in allGOs)
        {
            if (possibleBuilding.TryGetComponent(out IFF possIFF) && possibleBuilding.transform.root.tag == "Building")
            {
                //Debug.Log(possibleBuilding.transform.root.name);
                possibleBuilding.transform.root.GetComponentInChildren<House>().SetOwningCity(this);

                int currentAllegiance = iff.GetIFFAllegiance();
                possIFF.SetIFFAllegiance(currentAllegiance);
                buildingsInCity.Add(possIFF);

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

    }

    private void CheckIfCaptured()
    {
        if (timeSpentCapturing >= timeToCapture)
        {
            int newAllegiance = playerIFF.GetIFFAllegiance();
            iff.SetIFFAllegiance(newAllegiance);
            foreach (IFF buildingIFF in buildingsInCity)
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
    }


    private void CheckForHousesRemaining()
    {
        if (buildingsInCity.Count <= 0)
        {
            sr.color = Color.red;
        }
    }

    public void RemoveBuildingFromList(IFF deadThing)
    {
        //Debug.Log("removal called");
        buildingsInCity.Remove(deadThing);
    }
}