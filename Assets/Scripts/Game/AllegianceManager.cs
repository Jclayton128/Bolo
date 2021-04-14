using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllegianceManager : MonoBehaviour
{
    //Init
    [SerializeField] Sprite[] flagSource = null;

    //hood
    public int playerAllegiance = 3;

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<IFF>().SetIFFAllegiance(playerAllegiance);
        Debug.Log("Forcing Player to allegiance 3");
    }


    // Update is called once per frame
    void Update()
    {
    }

    public Sprite GetFlagOfAllegiance(int key)
    {
        return flagSource[key];
    }

}
