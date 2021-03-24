using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    //Attack class is a base class that all unit's attack logic is handled at 

    //init
    protected Movement move;
    protected ControlSource cs;
    [SerializeField] protected GameObject projectilePrefab;

    //param

    protected virtual void Start()
    {
        cs = GetComponentInParent<ControlSource>();
        move = transform.parent.GetComponentInChildren<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void LMBDown();
    public abstract void LMBUp();
    public abstract void RMBDown();
    public abstract void RMBUp();
}
