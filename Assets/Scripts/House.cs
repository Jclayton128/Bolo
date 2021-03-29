﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class House : MonoBehaviour
{
    //init
    SpriteRenderer sr;
    [SerializeField] Sprite[] possibleHouseSprites = null;
    CitySquare cs;

    //param

    //hood
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ChooseHouseImage();
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
        if (!cs) { return; }
        cs.RemoveBuildingFromList(gameObject.GetComponentInChildren<IFF>());
    }
}
