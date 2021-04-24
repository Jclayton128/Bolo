using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]


public class StealthHider : MonoBehaviour
{
    //init
    SpriteRenderer[] srs;
    Rigidbody2D rb;
    CircleCollider2D hiderColl;
    ControlSource cs;

    //param
    public float hiderRadius_Base;
    public float hiderGrowthRate = .5f; //per second;
    public float hiderShrinkRate = .2f; //per second;

    //hood
    public float hiderRadius_Modified;
    public float hiderRadius_TerrainModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        srs = transform.root.GetComponentsInChildren<SpriteRenderer>();
        MakeObjectInvisible();
        rb = transform.root.GetComponentInChildren<Rigidbody2D>();
        hiderColl = GetComponent<CircleCollider2D>();
        hiderColl.radius = hiderRadius_Base;
        cs = transform.root.GetComponentInChildren<ControlSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHiderRadiusInputBasedOnSpeed();

    }
    private void UpdateHiderRadiusInputBasedOnSpeed()
    {
        float vel = rb.velocity.magnitude;
        hiderRadius_TerrainModifier = GetTerrainModifier();
        hiderRadius_Modified = hiderRadius_Base * vel * hiderRadius_TerrainModifier;
        AdjustHiderRadius();
    }

    private float GetTerrainModifier()
    {
        int terrain = cs.currentTerrainType;

        if (terrain == 4) //road
        {
            hiderRadius_TerrainModifier = 1.25f;
            //Debug.Log("hiding in road");
            return hiderRadius_TerrainModifier;
        }
        if (terrain == 5)  //hills
        {
            hiderRadius_TerrainModifier = .5f;
            //Debug.Log("hiding in hills");
            return hiderRadius_TerrainModifier;
        }
        if (terrain == 6) //forest
        {
            hiderRadius_TerrainModifier = .75f;
            //Debug.Log("hiding in forest");
            return hiderRadius_TerrainModifier;
        }
        else
        {
            hiderRadius_TerrainModifier = 1;
            //Debug.Log("hiding in field");
            return hiderRadius_TerrainModifier;
        }


    }

    private void AdjustHiderRadius()
    {

        if (hiderRadius_Modified > hiderColl.radius)
        {
            Debug.Log("hider radius needs to grow");
            hiderColl.radius += hiderGrowthRate * Time.deltaTime;
        }
        if (hiderRadius_Modified < hiderColl.radius)
        {
            Debug.Log("hider radius needs to shrink");
            hiderColl.radius -= hiderShrinkRate * Time.deltaTime;
        }
        hiderColl.radius = Mathf.Clamp(hiderColl.radius, hiderRadius_Base / 4, hiderRadius_Base * 4);
    }

    public void MakeObjectInvisible()
    {
        if (transform.root.tag != "Player")
        {
            foreach (SpriteRenderer thisSR in srs)
            {
                thisSR.enabled = false;
            }
        }
    }

    public void MakeObjectVisible()
    {
        foreach (SpriteRenderer thisSR in srs)
        {
            thisSR.enabled = true;
        }
    }
}
