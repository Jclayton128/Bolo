using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MoneyHolder), typeof(IFF), typeof(HouseHolder))]

public class FactionLeader : MonoBehaviour
{
    //init
    AllegianceManager am;
    IFF iff;
    void Start()
    {
        am = FindObjectOfType<AllegianceManager>();
        iff = transform.root.GetComponentInChildren<IFF>();
        am.AddFactionLeaderToList(iff.GetIFFAllegiance(), this);
        //Debug.Log(gameObject.name + " attempted to add an FL for allegiance: " + iff.GetIFFAllegiance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
