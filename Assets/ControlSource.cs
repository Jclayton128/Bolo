using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSource : MonoBehaviour
{
    // Control Source class is intended to be the top-level of a unit. It is either the interaction level with the player, or
    // the level where an AI-controlled unit's inputs come from.

    //init
    protected Movement move;
    protected Attack attack;

    //param

    //hood
    public float horizComponent;
    public float vertComponent;
    public Vector3 mousePos = new Vector3(0, 0, 0);
    protected virtual void Start()
    {
        move = GetComponentInChildren<Movement>();
        attack = GetComponentInChildren<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
