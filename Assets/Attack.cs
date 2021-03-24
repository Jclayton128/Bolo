using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Attack class is a base class that all unit's attack logic is handled at 

    //init
    protected Movement move;
    protected ControlSource cs;
    protected virtual void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        move = transform.parent.GetComponentInChildren<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
