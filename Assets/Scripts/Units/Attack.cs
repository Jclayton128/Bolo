﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    //Attack class is a base class that all unit's attack logic is handled at 

    //init
    protected Movement move;
    protected ControlSource cs;
    [SerializeField] protected GameObject projectilePrefab = null;
    [SerializeField] protected AudioClip[] firingSounds = null;

    //param

    //hood
    protected AudioClip selectedFiringSound;

    protected virtual void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        move = transform.parent.GetComponentInChildren<Movement>();
        SelectFiringSound();
    }

    private void SelectFiringSound()
    {
        if (firingSounds.Length == 0) { return; }
        int random = UnityEngine.Random.Range(0, firingSounds.Length);
        selectedFiringSound = firingSounds[random];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void AttackCommence();
    public abstract void AttackRelease();

    public abstract float GetAttackRange();

}
