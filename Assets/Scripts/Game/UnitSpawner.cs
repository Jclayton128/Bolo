using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour
{
    //init
    UnitTracker ut;
    AllegianceManager am;
    CityManager cm;
    List<GameObject> unitsInQueue = new List<GameObject>();
    [SerializeField] Image[] queueStack = null;
    [SerializeField] Sprite[] unitIcons = null;
    [SerializeField] GameObject[] unitPrefabMenu = null;  //soldier, tank, missile truck, helo
    GameObject playerAtComputer;

    //param
    float timeBetweenSpawningUnits = 5f; //seconds
    int[] unitCosts = new int[] { 10, 50, 30, 100 };

    //hood
    float timeSinceLastSpawn = 0;

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
        ListenForKeyboardSummon();
        MeasureTimeForQueue();
    }

    private void MeasureTimeForQueue()
    {
        if (unitsInQueue.Count > 0)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= timeBetweenSpawningUnits)
            {
                SpawnNextUnitInQueue();
                timeSinceLastSpawn = 0;
            }
        }
    }

    private void ListenForKeyboardSummon()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            PlayerOrderUpNewUnit(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            PlayerOrderUpNewUnit(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            PlayerOrderUpNewUnit(2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            PlayerOrderUpNewUnit(3);
        }
    }

    public void PlayerOrderUpNewUnit(int chosenUnit)
    {
        //TODO check for sufficient funds

        if (unitsInQueue.Count >= queueStack.Length)
        {
            Debug.Log("queue is full!");
            //TODO play a 'bounce' sound to prevent ordering while the queue is full
            return;
        }
        else
        {
            GameObject newUnit = unitPrefabMenu[chosenUnit];
            int iffToSpawnWith = playerAtComputer.GetComponentInChildren<IFF>().GetIFFAllegiance();
            newUnit.GetComponentInChildren<IFF>().SetIFFAllegiance(iffToSpawnWith);
            unitsInQueue.Add(newUnit);
            AddIconToQueueStack(unitIcons[chosenUnit]);
        }
        
        //TODO withdraw unit costs from MoneyHolder

    }

    private void SpawnNextUnitInQueue()
    {
        GameObject unitBlueprint = unitsInQueue[0];
        int iffToSpawnWith = unitBlueprint.GetComponentInChildren<IFF>().GetIFFAllegiance();
        Vector3 spawnPos = cm.FindNearestCitySquare(playerAtComputer.transform, iffToSpawnWith).transform.position;
        GameObject newActualUnit = Instantiate(unitBlueprint, spawnPos, Quaternion.identity) as GameObject;
        Debug.Log("Made a new unit: " + newActualUnit.name);
        ut.AddUnitToTargetableList(newActualUnit);
        unitsInQueue.RemoveAt(0);
        DecrementQueueStack();      
    }

    private void AddIconToQueueStack(Sprite newIcon)
    {
        int openSpot = unitsInQueue.Count;
        queueStack[openSpot-1].sprite = newIcon;

    }

    private void DecrementQueueStack()
    {
        if (unitsInQueue.Count == 0)
        {
            queueStack[0].sprite = null;
            return;
        }
        for (int i = 0; i < unitsInQueue.Count; i++)
        {
            queueStack[i].sprite = queueStack[i + 1].sprite;
        }
        queueStack[unitsInQueue.Count].sprite = null;
    }

    public void AISpawnUnit(int chosenUnit, int unitIFF)
    {
        //TODO implement AI's ability to spawn
    }
}
