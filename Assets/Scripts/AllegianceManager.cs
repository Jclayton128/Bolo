using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllegianceManager : MonoBehaviour
{
    //Init
    [SerializeField] Sprite[] flagSource = null;

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
