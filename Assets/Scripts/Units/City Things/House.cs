using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(IFF))]
public class House : MonoBehaviour
{
    //init
    UnitTracker ut;
    AllegianceManager am;
    SpriteRenderer sr;
    [SerializeField] Sprite[] possibleHouseSprites = null;
    CitySquare cs;
    public IFF iff;

    //param
    public bool isHouse = true;


    //hood


    void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
        iff = GetComponentInChildren<IFF>();
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
    public void DyingActions()
    {
        //Debug.Log("dying actions called");
        if (!cs) { return; }
        cs.RemoveBuildingFromList(this);
        ut.RemoveUnitFromTargetableList(gameObject);
        if (!GetComponent<DefenseTurret>())
        {
            GameObject owner = am.GetFactionLeader(iff.GetIFFAllegiance()).gameObject;
            owner.GetComponent<HouseHolder>().DecrementHouseCount();
        }
    }

    public void SetHouseIFFAllegiance(int newIFF)
    {
        iff.SetIFFAllegiance(newIFF);
        //Debug.Log("asking AM for this iff: " + newIFF);
        if (!GetComponent<DefenseTurret>())
        {
            GameObject owner = am.GetFactionLeader(newIFF).gameObject;
            owner.GetComponent<HouseHolder>().IncrementHouseCount();
        }
    }

    public int GetHouseIFFAllegiance()
    {
        return iff.GetIFFAllegiance();
    }
}
