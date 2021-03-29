using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    //init
    [SerializeField] List<string> cityNames = new List<string>();
    CitySquare[] citySquares;
    void Start()
    {
        citySquares = FindObjectsOfType<CitySquare>();
    }


    public string GetRandomCityName()
    {
        int random = Random.Range(0, cityNames.Count);
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
