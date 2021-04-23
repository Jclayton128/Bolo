using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllegianceManager : MonoBehaviour
{
    //Init
    [SerializeField] GameObject dummyFactionLeaderPrefab = null;
    [SerializeField] Sprite[] flagSource = null;
    SortedList<int, FactionLeader> factionLeaders = new SortedList<int, FactionLeader>();

    //hood
    public int playerAllegiance = 1;

    private void Awake()
    {
        int amCount = FindObjectsOfType<AllegianceManager>().Length;
        if (amCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<IFF>().SetIFFAllegiance(playerAllegiance);

    }


    // Update is called once per frame
    void Update()
    {

    }
    public void AddFactionLeaderToList(int allegiance, FactionLeader fl) //call this when the Arena scene is loaded.
    {
        if (factionLeaders.ContainsKey(allegiance))
        {
            //Debug.Log("faction " + allegiance + " already has a leader!");
        }
        else
        {
            //Debug.Log("added faction leader for faction " + allegiance + ".");
            factionLeaders.Add(allegiance, fl);
        }
    }

    public FactionLeader GetFactionLeader(int iffAllegiance)
    {
        //Debug.Log($"I was asked for the faction leader for {iffAllegiance}");
        if (!factionLeaders.ContainsKey(iffAllegiance))
        {
            //Debug.Log($"A faction leader doesn't exist for {iffAllegiance}. Creating a dummy");
            GameObject dummy = Instantiate(dummyFactionLeaderPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            dummyFactionLeaderPrefab.GetComponentInChildren<IFF>().SetIFFAllegiance(iffAllegiance);
            factionLeaders[iffAllegiance] = dummy.GetComponent<FactionLeader>();
            return factionLeaders[iffAllegiance];
        }
        else
        {
            FactionLeader fl = factionLeaders[iffAllegiance];
            return fl;
        }

    }

    public Sprite GetFlagOfAllegiance(int key)
    {
        return flagSource[key];
    }



}
