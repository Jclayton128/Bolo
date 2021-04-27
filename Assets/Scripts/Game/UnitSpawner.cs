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
    [SerializeField] SpawnPanel sp = null;
    [SerializeField] GameObject[] unitPrefabMenu = null;
    GameObject playerAtComputer;

    void Start()
    {
        playerAtComputer = GameObject.FindGameObjectWithTag("Player");
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
            GameObject factionLeader = am.GetFactionLeader(am.GetPlayerIFF()).gameObject;
            CitySquare closestCSToPlayer = cm.FindNearestCitySquare(factionLeader.transform.transform, am.GetPlayerIFF());
            GameObject unit = Instantiate(unitPrefabMenu[0], closestCSToPlayer.transform.position, Quaternion.identity) as GameObject;
            unit.GetComponentInChildren<IFF>().SetIFFAllegiance(am.GetPlayerIFF());
        }
    }

    public void PlayerSpawnUnit(int chosenUnit)
    {
        if (!sp.isExtended) { return; }
        int iffToSpawnWith = playerAtComputer.GetComponentInChildren<IFF>().GetIFFAllegiance();
        Vector3 spawnPos = cm.FindNearestCitySquare(playerAtComputer.transform, iffToSpawnWith).transform.position;

        GameObject newUnit = Instantiate(unitPrefabMenu[chosenUnit], spawnPos, Quaternion.identity) as GameObject;
        newUnit.GetComponentInChildren<IFF>().SetIFFAllegiance(iffToSpawnWith);
        ut.AddUnitToTargetableList(newUnit);
        return;
    }

    public void AISpawnUnit(int chosenUnit, int unitIFF)
    {
        //TODO implement AI's ability to spawn
    }
}
