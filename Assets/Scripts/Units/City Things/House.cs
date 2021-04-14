using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class House : MonoBehaviour
{
    //init
    UnitTracker ut;
    SpriteRenderer sr;
    [SerializeField] Sprite[] possibleHouseSprites = null;
    CitySquare cs;

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
    public void DyingActions()
    {
        //Debug.Log("dying actions called");
        if (!cs) { return; }
        cs.RemoveBuildingFromList(gameObject.GetComponentInChildren<IFF>());
        ut.RemoveUnitFromTargetableList(gameObject);
    }
}
