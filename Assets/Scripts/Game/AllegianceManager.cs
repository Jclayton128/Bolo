using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllegianceManager : MonoBehaviour
{
    //Init
    [SerializeField] Sprite[] flagSource = null;
    SortedList<int, FactionLeader> factionLeaders = new SortedList<int, FactionLeader>();

    //hood
    public int playerAllegiance;

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
            Debug.Log("faction " + allegiance + " already has a leader!");
        }
        else
        {
            Debug.Log("added faction leader for faction " + allegiance + ".");
            factionLeaders.Add(allegiance, fl);
        }

    }

    public Sprite GetFlagOfAllegiance(int key)
    {
        return flagSource[key];
    }



}
