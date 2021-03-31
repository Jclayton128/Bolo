using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Compass : MonoBehaviour
{
    //init
    CityManager cm;
    GameObject player;
    TextMeshProUGUI cityNameTextBar;
    float maxCityDistance = 30f;

    //hood
    CitySquare nearestCS;
    float distToNearestCS;
    float initialScale;

    void Start()
    {
        cm = FindObjectOfType<CityManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        cityNameTextBar = GameObject.FindGameObjectWithTag("CityNameTextBar").GetComponent<TextMeshProUGUI>();
        initialScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        OrientCompassTowardsNearestCity();
        AdjustCompassSizeBasedOnDistanceToNearestCity();

    }

    private void AdjustCompassSizeBasedOnDistanceToNearestCity()
    {
        float dist = (nearestCS.transform.position - player.transform.position).magnitude;
        float factor = 1 - (dist / maxCityDistance);

        factor = Mathf.Clamp(factor, .3f, 1.0f);
        Debug.Log("post clamp factor: " + factor);
        transform.localScale = Vector3.one * factor * initialScale;
    }

    private void OrientCompassTowardsNearestCity()
    {
        nearestCS = cm.FindNearestCitySquare(player.transform);
        cityNameTextBar.text = nearestCS.cityName;
        float ang = cm.FindAngleToCitySquare(player.transform, nearestCS);
        Quaternion rot = Quaternion.Euler(0, 0, ang);
        transform.rotation = rot;
    }
}
