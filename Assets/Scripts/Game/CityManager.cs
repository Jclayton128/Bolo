using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CityManager : MonoBehaviour
{
    //init
    public List<string> cityNames = new List<string>();
    CitySquare[] citySquares;
    Slider cityCaptureSlider;
    TextMeshProUGUI cityNameTextBar;
    GameObject player;

    //hood
    CitySquare closestCS;

    private void Awake()
    {
        citySquares = FindObjectsOfType<CitySquare>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
    }



    private void Update()
    {
        closestCS = FindNearestCitySquare(player.transform);
        DisplayCityName();
        BoldCityNameIfWithinRange();
      
    }

    public int GetNumberOfCitySquares()
    {
        return citySquares.Length;
    }

    public CitySquare GetCitySquare(int i)
    {
        return citySquares[i];
    }

    private void DisplayCityName()
    {
        cityNameTextBar.text = closestCS.cityName;
    }
    private void BoldCityNameIfWithinRange()
    {
        float dist = (player.transform.position - closestCS.transform.position).magnitude;
        if (dist <= closestCS.cityRadius)
        {
            cityNameTextBar.fontStyle = TMPro.FontStyles.Bold;
        }
        else
        {
            cityNameTextBar.fontStyle = TMPro.FontStyles.Normal;
        }
    }

    public string GetRandomCityName()
    {
        int random = UnityEngine.Random.Range(0, cityNames.Count);
        string chosenName = cityNames[random];
        //Debug.Log(chosenName);
        cityNames.Remove(chosenName);
        return chosenName;
    }

    public CitySquare FindNearestCitySquare(Transform sourceTransform)
    {
        citySquares = FindObjectsOfType<CitySquare>();
        CitySquare closestCitySquare = null;
        float distance = Mathf.Infinity;
        foreach (CitySquare currentCS in citySquares)
        {
            float diff = (currentCS.transform.position - sourceTransform.position).magnitude;
            if (diff < distance)
            {
                closestCitySquare = currentCS;
                distance = diff;
            }
        }
        return closestCitySquare;       
    }

    public CitySquare FindNearestCitySquare(Transform sourceTransform, int allegianceToLookFor)
    {
        citySquares = FindObjectsOfType<CitySquare>();
        CitySquare closestCitySquare = null;
        float distance = Mathf.Infinity;
        foreach (CitySquare currentCS in citySquares)
        {
            if (currentCS.GetComponentInChildren<IFF>().GetIFFAllegiance() != allegianceToLookFor) { continue; }
            float diff = (currentCS.transform.position - sourceTransform.position).magnitude;
            if (diff < distance)
            {
                closestCitySquare = currentCS;
                distance = diff;
            }
        }
        //Debug.Log($"Closest CS for {allegianceToLookFor} is at {closestCitySquare.transform.position}");
        return closestCitySquare;
    }

    public CitySquare FindNearestCitySquare_IgnoreIFF(Transform sourceTransform, int allegianceToIgnore)
    {
        citySquares = FindObjectsOfType<CitySquare>();
        CitySquare closestCitySquare = null;
        float distance = Mathf.Infinity;
        foreach (CitySquare currentCS in citySquares)
        {
            if (currentCS.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore) { continue; }
            float diff = (currentCS.transform.position - sourceTransform.position).magnitude;
            if (diff < distance)
            {
                closestCitySquare = currentCS;
                distance = diff;
            }
        }
        //Debug.Log($"Closest CS for {allegianceToLookFor} is at {closestCitySquare.transform.position}");
        return closestCitySquare;
    }


    public bool TryFindNearestCitySquare(Transform sourceTransform, int allegianceToIgnore, out CitySquare closestCitySquare)
    {
        citySquares = FindObjectsOfType<CitySquare>();
        bool foundSomething = false;
        closestCitySquare = null;
        float distance = Mathf.Infinity;
        foreach (CitySquare currentCS in citySquares)
        {
            if (currentCS.GetComponentInChildren<IFF>().GetIFFAllegiance() == allegianceToIgnore)
            {               
                foundSomething = false;
                continue;
            }
            float diff = (currentCS.transform.position - sourceTransform.position).magnitude;
            if (diff < distance)
            {
                closestCitySquare = currentCS;
                distance = diff;
            }
        }

        if (closestCitySquare)
        {
            foundSomething = true;
        }
        return foundSomething;
    }

    public float FindAngleToCitySquare(Transform sourceTransform, CitySquare targetCitySquare)
    {
        Vector3 dir = targetCitySquare.transform.position - sourceTransform.position;
        float ang = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);
        return ang;
    }

    public bool TryGetCitySquareWithinRange(Transform sourceTransform, float searchRange, int iffToIgnore, out CitySquare outCS)
    {
        outCS = null;
        bool foundSomething = false;
        foreach (CitySquare cs in citySquares)
        {
            float dist = (cs.transform.position - sourceTransform.position).magnitude;
            if (dist >= searchRange) { continue; }
            if (cs.transform.root.GetComponentInChildren<IFF>().GetIFFAllegiance() == iffToIgnore) { continue; }
            else
            {
                foundSomething = true;
                outCS = cs;
            }
        }
        return foundSomething;
    }


}
