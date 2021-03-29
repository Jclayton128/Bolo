using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNamesManager : MonoBehaviour
{
    //init
    [SerializeField] List<string> cityNames = new List<string>();
    void Start()
    {

    }


    public string GetRandomCityName()
    {
        int random = Random.Range(0, cityNames.Count);
        string chosenName = cityNames[random];
        Debug.Log(chosenName);
        cityNames.Remove(chosenName);
        return chosenName;
    }
}
