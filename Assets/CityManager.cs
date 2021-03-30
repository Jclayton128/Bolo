using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CityManager : MonoBehaviour
{
    //init
    [SerializeField] List<string> cityNames = new List<string>();
    CitySquare[] citySquares;
    Slider cityCaptureSlider;
    TextMeshProUGUI cityNameTextBar;
    GameObject player;

    //hood
    CitySquare closestCS;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        citySquares = FindObjectsOfType<CitySquare>();
        SetupCityCaptureSlider();
        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
    }


    private void SetupCityCaptureSlider()
    {
        cityCaptureSlider = GameObject.FindGameObjectWithTag("CCB").GetComponent<Slider>();
        cityCaptureSlider.maxValue = citySquares[0].timeToCapture;
        cityCaptureSlider.minValue = 0;
        cityCaptureSlider.value = 0;
    }
    private void Update()
    {
        closestCS = FindNearestCitySquare(player.transform);
        DisplayCityName();
        BoldCityNameIfWithinRange();
        UpdateCaptureBarWithClosestCityInfo();
        
    }

    private void DisplayCityName()
    {
        cityNameTextBar.text = closestCS.cityName;
    }

    private void UpdateCaptureBarWithClosestCityInfo()
    {
        cityCaptureSlider.value = closestCS.timeSpentCapturing;
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

    public float FindAngleToCitySquare(Transform sourceTransform, CitySquare targetCitySquare)
    {
        Vector3 dir = targetCitySquare.transform.position - sourceTransform.position;
        float ang = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward);
        return ang;
    }


}
