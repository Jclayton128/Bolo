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


    //hood


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isHouse)
        {
            ChooseHouseImage();
        }
        ut = FindObjectOfType<UnitTracker>();
        ut.AddUnitToTargetableList(gameObject);
    }
    private void ChooseHouseImage()
    {
        int rand = UnityEngine.Random.Range(0, possibleHouseSprites.Length);
        sr.sprite = possibleHouseSprites[rand];
    }

    // Update is called once per frame
    void Update()
    {

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
            //Debug.Log($"{am} was asked for the faction leader for {newIFF}");
            GameObject owner = am.GetFactionLeader(newIFF).gameObject;
            owner.GetComponent<HouseHolder>().IncrementHouseCount();
        }
    }


    public void DyingActions()
    {
        if (!cs) { return; }
        if (!GetComponent<DefenseTurret>())
        {
            GameObject owner = am.GetFactionLeader(iff.GetIFFAllegiance()).gameObject;
            owner.GetComponent<HouseHolder>().DecrementHouseCount();
        }
        cs.RemoveBuildingFromList(this);
        ut.RemoveUnitFromTargetableList(gameObject);
    }
}
