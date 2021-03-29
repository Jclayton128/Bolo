using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    //init
    CityManager cm;
    GameObject player;
    TextMeshProUGUI cityNameTextBar;

    //hood
    CitySquare nearestCS;
    void Start()
    {
        cm = FindObjectOfType<CityManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestCS = cm.FindNearestCitySquare(player.transform);
        cityNameTextBar.text = nearestCS.cityName;
        float ang = cm.FindAngleToCitySquare(player.transform, nearestCS);
        Quaternion rot = Quaternion.Euler(0,0,ang);
        transform.rotation = rot;

    }
}
