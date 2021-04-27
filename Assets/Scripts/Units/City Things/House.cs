using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(IFF))]
public class House : MonoBehaviour
{
    //init
    UnitTracker ut;
    public AllegianceManager am;
    SpriteRenderer sr;
    [SerializeField] Sprite[] possibleHouseSprites = null;
    CitySquare cs;
    public IFF iff;


    //param
    public bool isHouse = true;
    float timeBetweenMoneyDrops = 5f;
    int amountOfMoneyOnEachDrop = 1;

    //hood
    public GameObject owner;
    float timeSinceLastMoneyDrop;
    


    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isHouse)
        {
            ChooseHouseImage();
        }
        ut = FindObjectOfType<UnitTracker>();
        ut.AddUnitToTargetableList(gameObject);
        UpdateCurrentOwner();
        timeSinceLastMoneyDrop = UnityEngine.Random.Range(0, timeBetweenMoneyDrops);
    }
    private void ChooseHouseImage()
    {
        int rand = UnityEngine.Random.Range(0, possibleHouseSprites.Length);
        sr.sprite = possibleHouseSprites[rand];
    }

    // Update is called once per frame
    void Update()
    {
        GenerateMoneyForOwner();
    }

    private void GenerateMoneyForOwner()
    {
        timeSinceLastMoneyDrop -= Time.deltaTime;
        if (timeSinceLastMoneyDrop <= 0)
        {
            UpdateCurrentOwner();
            owner.GetComponent<MoneyHolder>().AddMoney(amountOfMoneyOnEachDrop);
            timeSinceLastMoneyDrop = timeBetweenMoneyDrops;
        }
    }

    public void UpdateCurrentOwner()
    {
        owner = am.GetFactionLeader(iff.GetIFFAllegiance()).gameObject;
    }
    public void SetOwningCity(CitySquare citysq)
    {
        cs = citysq;
    }    
    public void SetHouseIFFAllegiance(int newIFF)
    {
        iff.SetIFFAllegiance(newIFF);

        if (!GetComponent<DefenseTurret>())
        {

            if (owner) //don't decrement if there isn't a previous owner to decrement from
            {
                owner.GetComponent<HouseHolder>().DecrementHouseCount();  //owner reference should still be the old owner
            }
            owner = am.GetFactionLeader(newIFF).gameObject; //now owner reference becomes the new owner.
            owner.GetComponent<HouseHolder>().IncrementHouseCount();
        }
    }


    public void DyingActions()
    {
        if (!cs) { return; }
        if (!GetComponent<DefenseTurret>())
        {
            UpdateCurrentOwner();
            owner.GetComponent<HouseHolder>().DecrementHouseCount();
        }
        cs.RemoveBuildingFromList(this);
        ut.RemoveUnitFromTargetableList(gameObject);
    }
}
