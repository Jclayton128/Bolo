using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityNamesManager : MonoBehaviour
{
    //init
    [SerializeField] string[] possibleCityNames = null;
    List<string> cityNames = new List<string>();
    void Start()
    {
        foreach (string str in possibleCityNames)
        {
            cityNames.Add(str);
        }
    }


    public string GetRandomCityName()
    {
        int random = UnityEngine.Random.Range(0, cityNames.Count);
        string chosenName = cityNames[random];
        cityNames.Remove(chosenName);
        return chosenName;
    }
}
