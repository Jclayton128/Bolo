using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    //init
    UnitTracker ut;
    AllegianceManager am;
    CityManager cm;
    [SerializeField] GameObject[] spawnableUnits = null;

    void Start()
    {
        ut = FindObjectOfType<UnitTracker>();
        am = FindObjectOfType<AllegianceManager>();
        cm = FindObjectOfType<CityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenForCheatSummon();
    }

    private void ListenForCheatSummon()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            GameObject factionLeader = am.GetFactionLeader(am.playerAllegiance).gameObject;
            CitySquare closestCSToPlayer = cm.FindNearestCitySquare(factionLeader.transform.transform, am.playerAllegiance);
            GameObject unit = Instantiate(spawnableUnits[0], closestCSToPlayer.transform.position, Quaternion.identity) as GameObject;
            unit.GetComponentInChildren<IFF>().SetIFFAllegiance(am.playerAllegiance);
        }
    }
}
