using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement class is the base class that all unit movements derive from;

    //init
    protected ControlSource cs;
    protected Attack attack;
    protected Rigidbody2D rb;
    
    protected virtual void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        attack = transform.parent.GetComponentInChildren<Attack>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
