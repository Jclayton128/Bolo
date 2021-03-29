using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent (typeof(IFF))]

public class CitySquare : MonoBehaviour
{
    //init
    IFF iff;
    SpriteRenderer sr;
    TextMeshProUGUI cityNameTextBar;
    GameObject player;


    //param
    float cityRadius = 5f;

    //hood
    string cityName;
    List<House> housesInCity = new List<House>();
    int allegiance;

    void Start()
    {
        player = Finder.FindNearestGameObjectWithTag(transform, "Player");
        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
        iff = GetComponentInChildren<IFF>();
        allegiance = iff.GetIFFAllegiance();
        SelectCityName();
        FindHousesWithinCityAndAdjustAllegiance();        
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
        foreach (GameObject possibleHouse in allGOs)
        {
            if (possibleHouse.TryGetComponent(out House house))
            {
                possibleHouse.GetComponentInChildren<IFF>().SetIFFAllegiance(allegiance);
                housesInCity.Add(house);
                continue;
            }
        }
        Debug.Log(housesInCity.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //HandleBeingCaptured();
        //CheckForHousesRemaining();
        DisplayCityNameIfWithinRange();
    }

    private void DisplayCityNameIfWithinRange()
    {
        float dist = (player.transform.position - transform.position).magnitude;
        if (dist <= cityRadius)
        {
            cityNameTextBar.text = cityName;
        }
        else
        {
            cityNameTextBar.text = " ";
        }
    }

    private void CheckForHousesRemaining()
    {
        if (housesInCity.Count <= 0)
        {
            sr.color = Color.red;
        }
    }

    public void RemoveHouseFromList(House deadHouse)
    {
        housesInCity.Remove(deadHouse);
    }
}
