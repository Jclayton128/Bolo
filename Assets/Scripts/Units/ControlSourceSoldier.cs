using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSourceSoldier : ControlSource
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        DetermineMissionAtStart();
    }

    private void DetermineMissionAtStart()
    {
        //get closest city to the player. if its friendly, then just start moving towards the player. if cc2p is non-friendly, then move towards it.
        //cm.FindNearestCitySquare();
    }

    // Update is called once per frame
    void Update()
    {
        base.Start();
    }

}
